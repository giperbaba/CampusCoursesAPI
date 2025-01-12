using repassAPI.Models.Enums;

namespace repassAPI.Models.Response;

public class CoursePreviewResponse(
    string id,
    string? name,
    int startYear,
    int maxStudentsCount,
    int remainingSlotsCount,
    CourseStatus status,
    Semester semester)
{
    public string Id { get; init; } = id;
    public string? Name { get; set; } = name;
    public int StartYear { get; set; } = startYear;
    public int MaxStudentsCount { get; set; } = maxStudentsCount;
    public int RemainingSlotsCount { get; set; } = remainingSlotsCount;
    public CourseStatus Status { get; set; } = status;
    public Semester Semester { get; set; } = semester;
}