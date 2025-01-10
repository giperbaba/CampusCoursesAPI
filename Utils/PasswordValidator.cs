using System.ComponentModel.DataAnnotations;

namespace repassAPI.Utils;

public class PasswordValidator
{
    public static bool IsValid(string password, string confirmPassword)
    {
        return password == confirmPassword;
    }
}