using repassAPI.Models.Enums;

namespace repassAPI.Models.Response;

public class CampusCoursePreviewResponse
{
    public Guid Id { get; init; }
    public string? Name { get; set; }
    public int StartYear { get; set; }
    public int MaxStudentsCount { get; set; }
    public int RemainingSlotsCount { get; set; }
    public CourseStatus Status { get; set; }
    public Semester Semester { get; set; }
}