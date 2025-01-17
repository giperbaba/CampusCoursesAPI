using System.ComponentModel.DataAnnotations;
using repassAPI.Constants;
using repassAPI.Models.Enums;

namespace repassAPI.Models.Request;

public class CourseStudentEditMarkRequest
{
    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [EnumDataType(typeof(MarkType))]
    public MarkType MarkType { get; set; }
    
    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [EnumDataType(typeof(StudentMark))]
    public StudentMark Mark { get; set; }
}