using System.ComponentModel.DataAnnotations;
using repassAPI.Constants;
using repassAPI.Models.Enums;

namespace repassAPI.Models.Request;

public class CourseEditRequest
{
    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [StringLength(1000, MinimumLength = 1)]
    public string? Name { get; set; } 
    
    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [Range(2000, 2029, ErrorMessage = ErrorMessages.InvalidStartCoursesYear)]
    public int StartYear { get; set; }
    
    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [Range(1, 200, ErrorMessage = ErrorMessages.InvalidStudentsAmount)]
    public int MaxStudentsCount { get; set; }
    
    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [EnumDataType(typeof(Semester))]
    public Semester Semester { get; set; }
    
    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [StringLength(1000, MinimumLength = 1)]
    public string Requirements { get; set; }
    
    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [StringLength(1000, MinimumLength = 1)]
    public string Annotations { get; set; }
}