namespace repassAPI.Constants;

public class RegularExpression
{
    public const string Password = "(?=.*[0-9])[0-9a-zA-Z_]{6,}";
    public const string Email = @"([a-zA-Z0-9._-]+@[a-zA-Z0-9._-]+\.[a-zA-Z0-9_-]+)$";
}