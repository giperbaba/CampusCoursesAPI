using System.ComponentModel.DataAnnotations;
using repassAPI.Constants;
using repassAPI.Models.Enums;

namespace repassAPI.Models.Response;

public class CourseStudentResponse(
    Guid id,
    string name,
    string email,
    StudentStatus status,
    StudentMark midtermResult,
    StudentMark finalResult)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public StudentStatus Status { get; set; } = status;
    public StudentMark MidtermResult { get; set; } = midtermResult;
    public StudentMark FinalResult { get; set; } = finalResult;
}