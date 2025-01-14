using repassAPI.Models.Request;
using repassAPI.Models.Response;

namespace repassAPI.Services.Interfaces;

public interface ICourseService
{
    //admin
    public Task<CoursePreviewResponse> CreateCourse(string groupId, CourseCreateRequest request);
    public Task<CourseDetailedResponse> EditCourse(string courseId, CourseEditRequest request);
    public Task<CoursePreviewResponse> DeleteCourse(string id);
    
    //admin, main teacher
    public Task<CourseDetailedResponse> AddTeacherToCourse(string courseId, CourseAddTeacherRequest request);
    
    //admin, teacher
    public Task<CourseDetailedResponse> EditCourseStatus(string courseId, CourseEditStatusRequest request);
    public Task<CourseDetailedResponse> EditCourseReqAndAnnotations(string courseId,
        CourseEditReqAndAnnotationsRequest request);
    
    //anybody
    public Task<IResult> SignUp(string courseId, string userId);
    public Task<CourseDetailedResponse> GetCourseDetailedInfo(string courseId);
    
    
    public bool IsUserMainTeacher(string courseId, string userId);
    public bool IsUserTeacher(string courseId, string userId);
}