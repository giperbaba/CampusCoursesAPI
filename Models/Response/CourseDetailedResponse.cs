using repassAPI.Models.Enums;

namespace repassAPI.Models.Response;

public class CourseDetailedResponse(
    string id,
    string? name,
    int startYear,
    int maxStudentsCount,
    int studentsEnrolledCount,
    int studentsInQueueCount,
    string requirements,
    string annotations,
    CourseStatus status,
    Semester semester,
    IEnumerable<CourseStudentResponse> students,
    IEnumerable<CourseTeacherResponse> teachers,
    IEnumerable<CourseNotificationResponse> notifications)
{
    public string Id { get; init; } = id;
    public string? Name { get; set; } = name;
    public int StartYear { get; set; } = startYear;
    public int MaxStudentsCount { get; set; } = maxStudentsCount;

    public int StudentsEnrolledCount { get; set; } = studentsEnrolledCount; 
    public int StudentsInQueueCount { get; set; } = studentsInQueueCount; 

    public string Requirements { get; set; } = requirements;
    public string Annotations { get; set; } = annotations;

    public CourseStatus Status { get; set; } = status;
    public Semester Semester { get; set; } = semester;

    public IEnumerable<CourseStudentResponse> Students { get; set; } = students;
    public IEnumerable<CourseTeacherResponse> Teachers { get; set; } = teachers;

    public IEnumerable<CourseNotificationResponse> Notifications { get; set; } = notifications;
}