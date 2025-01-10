using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace repassAPI.Constants;

public class Tokens
{
    private readonly IConfiguration _configuration;
    public SecurityKey AccessTokenKey { get; set; }
    public SecurityKey RefreshTokenKey { get; set; }
    public int AccessTokenExpireMinutes { get; set; }
    public int RefreshTokenExpireMinutes { get; set; }
    
    public Tokens(IConfiguration configuration)
    {
        _configuration = configuration;
        InitKeys();
        InitExpiredTimes();
    }

    private void InitKeys()
    {
        AccessTokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:AccessKey").Value!));
        RefreshTokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:RefreshKey").Value!));
        Console.WriteLine(AccessTokenKey.ToString());
    }
    
    private void InitExpiredTimes()
    {
        AccessTokenExpireMinutes = int.Parse(_configuration.GetSection("AppSettings:AccessTokenExpireMinutes").Value!);
        RefreshTokenExpireMinutes = int.Parse(_configuration.GetSection("AppSettings:RefreshTokenExpireMinutes").Value!);
    }
}