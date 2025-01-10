using repassAPI.Entities;

namespace repassAPI.Services.Interfaces;

public interface ITokenService
{
    void SaveRefreshToken(User user, string refreshToken);
    void RemoveTokens(User user,  string? accessToken);
    Task CheckIsAccessTokenBanned(string token);
    string GetEmailFromRefreshToken(string? refreshToken);
}