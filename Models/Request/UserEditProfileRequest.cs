using System.ComponentModel.DataAnnotations;
using repassAPI.Constants;
using repassAPI.Utils;

namespace repassAPI.Models.Request;

public class UserEditProfileRequest(string fullName, DateTime birthdate)
{
    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [StringLength(1000, MinimumLength = 1)]
    public string FullName { get; } = fullName;

    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [DatetimeValidator(ErrorMessage = ErrorMessages.IncorrectDate)]
    public DateTime BirthDate { get; } = birthdate; 
}