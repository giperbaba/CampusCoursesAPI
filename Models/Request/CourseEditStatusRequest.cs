using System.ComponentModel.DataAnnotations;
using repassAPI.Constants;
using repassAPI.Models.Enums;

namespace repassAPI.Models.Request;

public class CourseEditStatusRequest
{
    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [EnumDataType(typeof(CourseStatus))]
    public CourseStatus status { get; set; }
}