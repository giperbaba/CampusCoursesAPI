using System.ComponentModel.DataAnnotations;
using repassAPI.Constants;
using repassAPI.Utils;

namespace repassAPI.Models.Request;
public class UserRegisterRequest(
    string fullName,
    DateTime birthdate,
    string email,
    string password,
    string confirmPassword)
{
    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [StringLength(1000, MinimumLength = 1)]
    public string FullName { get; } = fullName;

    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [DatetimeValidator(ErrorMessage = ErrorMessages.IncorrectDate)]
    public DateTime BirthDate { get; } = birthdate; 
    
    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [MinLength(1)]
    [RegularExpression(pattern: RegularExpression.Email, ErrorMessage = ErrorMessages.IncorrectEmailFormat)]
    [EmailAddress(ErrorMessage = ErrorMessages.IncorrectEmailFormat)]
    public string Email { get; } = email;

    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [RegularExpression(pattern: RegularExpression.Password, ErrorMessage = ErrorMessages.IncorrectPasswordFormat)]
    [MinLength(6)]
    public string Password { get; } = password;

    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [MinLength(6)]
    public string ConfirmPassword { get; } = confirmPassword;
}