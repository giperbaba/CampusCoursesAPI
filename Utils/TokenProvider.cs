using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using repassAPI.Constants;
using repassAPI.Data;
using repassAPI.Entities;
using repassAPI.Exceptions;

namespace repassAPI.Utils;

public class TokenProvider
{
    private readonly DatabaseContext _context;
    private readonly Tokens _tokens;
    
    public TokenProvider(DatabaseContext context,  Tokens tokens)
    {
        _context = context;
        _tokens = tokens;
    }
    
    public string GenerateAccessToken(User user)
    {
        return CreateToken(user, _tokens.AccessTokenKey, _tokens.AccessTokenExpireMinutes);
    }
    
    public string GenerateRefreshToken(User user)
    {
        return CreateToken(user, _tokens.RefreshTokenKey, _tokens.RefreshTokenExpireMinutes);
    }
    
    private string CreateToken(User user, SecurityKey key, int expireMinutes)
    { 
        var claims = new List<Claim> {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Id.ToString())
        };

        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            audience: "apiAudience",
            issuer: "apiIssuer",
            claims: claims,
            expires: DateTime.Now.AddMinutes(expireMinutes),
            signingCredentials: signingCredentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool IsRefreshTokenValid(User user, string refreshToken)
    {
        var token = _context.RefreshTokens.FirstOrDefault(t => t.Token == refreshToken);
        if (token == null)
        {
            return false;
        }
        CheckTokenEmail(token, user);
        CheckTokenExpired(token);   
        return true;
    }
    
    private void CheckTokenEmail(RefreshToken token, User user)
    {
        if (token.Email != user.Email)
        {
            throw new InvalidTokenException(ErrorMessages.InvalidToken);
        }
    }
    
    private void CheckTokenExpired(RefreshToken token)
    {
        if (token.IsExpired)
        {
            throw new InvalidTokenException(ErrorMessages.InvalidToken);
        }
    }
}