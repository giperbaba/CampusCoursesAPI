namespace repassAPI.Models.Response;

public class AccessTokenResponse(string accessToken)
{
    public string AccessToken { get; init; }= accessToken;
}