using Microsoft.EntityFrameworkCore;
using repassAPI.Constants;
using repassAPI.Data;
using repassAPI.Entities;
using repassAPI.Exceptions;
using repassAPI.Services.Interfaces;

namespace repassAPI.Services.Impl;

public class TokenService: ITokenService
{
    private readonly DatabaseContext _context;
    private readonly Tokens _tokens;
    
    public TokenService(DatabaseContext context, Tokens tokens)
    {
        _context = context;
        _tokens = tokens;
    }

    public void SaveRefreshToken(User user, string refreshToken)
    {
        var token = new RefreshToken
        {
            Token = refreshToken,
            Email = user.Email,
            Expires = DateTime.UtcNow.AddMinutes(_tokens.RefreshTokenExpireMinutes)
        };
        _context.RefreshTokens.Add(token);
        _context.SaveChanges();
    }
    
    public void RemoveTokens(User user, string? accessToken)
    {
        List<RefreshToken> tokens = _context.RefreshTokens.Where(t => t.Email == user.Email).ToList();
        if (tokens.Count != 0)
        {
            _context.RefreshTokens.RemoveRange(tokens);
            
            var bannedAccessToken = new AccessToken
            {
                Token = accessToken
            };
            _context.BannedTokens.Add(bannedAccessToken);
            
            _context.SaveChanges();
        }
    }
    
    public async Task CheckIsAccessTokenBanned(string accessToken)
    {
        bool isBanned = await _context.BannedTokens.AnyAsync(t => t.Token == accessToken);
        if (isBanned)
            throw new InvalidTokenException(ErrorMessages.InvalidToken);
    }
    
    public string GetEmailFromRefreshToken(string? refreshToken)
    {
        var token = _context.RefreshTokens.FirstOrDefault(t => t.Token == refreshToken);
        CheckFoundRefreshToken(token);
        return token?.Email;
    }

    private void CheckFoundRefreshToken(RefreshToken? refreshToken)
    {
        if (refreshToken == null || refreshToken.Expires < DateTime.UtcNow)
        {
            throw new InvalidTokenException(ErrorMessages.InvalidToken);
        }
    }
}