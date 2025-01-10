namespace repassAPI.Models.Response;

public class TokensResponse(string accessToken, string refreshToken)
{
    public string AccessToken { get; set; } = accessToken;
    public string RefreshToken { get; set; } = refreshToken;
}