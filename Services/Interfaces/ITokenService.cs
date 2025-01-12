using repassAPI.Entities;

namespace repassAPI.Services.Interfaces;

public interface ITokenService
{
    public void SaveRefreshToken(User user, string refreshToken);
    public void RemoveTokens(User user,  string? accessToken);
    public Task CheckIsAccessTokenBanned(string token);
    public string GetEmailFromRefreshToken(string? refreshToken);
}