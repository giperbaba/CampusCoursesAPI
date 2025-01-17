using System.ComponentModel.DataAnnotations;
using repassAPI.Constants;
using repassAPI.Models.Enums;

namespace repassAPI.Models.Request;

public class CourseStudentEditStatusRequest
{
    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [EnumDataType(typeof(StudentStatus))]
    public StudentStatus Status { get; set; }
}

