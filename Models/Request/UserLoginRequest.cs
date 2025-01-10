using System.ComponentModel.DataAnnotations;
using repassAPI.Constants;

namespace repassAPI.Models.Request;

public class UserLoginRequest(string email, string password)
{
    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [EmailAddress(ErrorMessage = ErrorMessages.IncorrectEmailFormat)]
    public string Email { get; set; } = email;

    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [MinLength(6, ErrorMessage = ErrorMessages.IncorrectPasswordFormat)]
    public string Password { get; set; } = password;
}