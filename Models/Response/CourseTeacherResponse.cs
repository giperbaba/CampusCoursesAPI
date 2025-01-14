namespace repassAPI.Models.Response;

public class CourseTeacherResponse(string name, string email, bool isMain)
{
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public bool IsMain { get; set; } = isMain;
}