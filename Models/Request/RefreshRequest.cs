namespace repassAPI.Models.Request;

public class RefreshRequest(string refreshToken)
{
    public string RefreshToken { get; set; } = refreshToken;
}