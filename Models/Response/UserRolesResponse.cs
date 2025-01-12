namespace repassAPI.Models.Response;

public class UserRolesResponse(bool isTeacher, bool isStudent, bool isAdmin)
{
    public bool IsTeacher { get; set; } = isTeacher;
    public bool IsStudent { get; set; } = isStudent;
    public bool IsAdmin { get; set; } = isAdmin;
}