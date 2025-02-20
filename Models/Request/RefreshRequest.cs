using System.ComponentModel.DataAnnotations;
using repassAPI.Constants;

namespace repassAPI.Models.Request;

public class RefreshRequest(string refreshToken)
{
    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    public string RefreshToken { get; set; } = refreshToken;
}