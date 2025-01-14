using System.ComponentModel.DataAnnotations;
using repassAPI.Constants;

namespace repassAPI.Models.Request;

public class CourseAddTeacherRequest
{
    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    public string userId { get; set; }
}