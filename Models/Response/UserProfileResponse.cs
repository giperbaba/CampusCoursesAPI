using System.ComponentModel.DataAnnotations;
using repassAPI.Constants;
using repassAPI.Utils;

namespace repassAPI.Models.Response;

public class UserProfileResponse(string fullName, string email, DateTime birthdate)
{
    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [StringLength(1000, MinimumLength = 1)]
    public string FullName { get; set; } = fullName;

    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [MinLength(1)]
    [RegularExpression(pattern: RegularExpression.Email, ErrorMessage = ErrorMessages.IncorrectEmailFormat)]
    [EmailAddress(ErrorMessage = ErrorMessages.IncorrectEmailFormat)]
    public string Email { get; set; } = email;

    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [DatetimeValidator(ErrorMessage = ErrorMessages.IncorrectDate)]
    public DateTime BirthDate { get; set; } = birthdate;
}