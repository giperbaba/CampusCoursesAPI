using repassAPI.Models.Request;
using repassAPI.Models.Response;

namespace repassAPI.Services.Interfaces;

public interface ICourseService
{
    public Task<CoursePreviewResponse> CreateCourse(string groupId, CourseCreateRequest courseCreate);
}