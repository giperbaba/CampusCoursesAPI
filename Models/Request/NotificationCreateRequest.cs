using System.ComponentModel.DataAnnotations;
using repassAPI.Constants;

namespace repassAPI.Models.Request;

public class NotificationCreateRequest
{
    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [StringLength(1000, MinimumLength = 1)]
    public string Text { get; set; }
    
    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    public bool IsImportant { get; set; } 
}
