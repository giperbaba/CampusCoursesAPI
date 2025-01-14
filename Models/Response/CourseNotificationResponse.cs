using System.ComponentModel.DataAnnotations;
using repassAPI.Constants;

namespace repassAPI.Models.Response;

public class CourseNotificationResponse(string text, bool isImportant = false)
{
    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [StringLength(1000, MinimumLength = 1)]
    public string Text { get; set; } = text;
    
    public bool IsImportant { get; set; } = isImportant;
}