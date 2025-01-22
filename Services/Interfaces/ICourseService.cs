using repassAPI.Models.Enums;
using repassAPI.Models.Request;
using repassAPI.Models.Response;

namespace repassAPI.Services.Interfaces;

public interface ICourseService
{
    //admin
    public Task<CoursePreviewResponse> CreateCourse(Guid groupId, CourseCreateRequest request);
    public Task<CourseDetailedResponse> EditCourse(string courseId, CourseEditRequest request);
    public Task<CoursePreviewResponse> DeleteCourse(string id);
    
    //admin, main teacher
    public Task<CourseDetailedResponse> AddTeacherToCourse(string courseId, CourseAddTeacherRequest request);
    
    //admin, teacher
    public Task<CourseDetailedResponse> CreateNewNotification(string courseId,
        NotificationCreateRequest notificationCreateRequest);

    public Task<CourseDetailedResponse> EditCourseStatus(string courseId, CourseEditStatusRequest request);
    public Task<CourseDetailedResponse> EditCourseReqAndAnnotations(string courseId,
        CourseEditReqAndAnnotationsRequest request);
    public Task<CourseDetailedResponse> EditStudentMark(string courseId, string studentId,
        CourseStudentEditMarkRequest editMarkRequest);
    public Task<CourseDetailedResponse> EditStudentStatus(string courseId, string studentId,
        CourseStudentEditStatusRequest editStatusRequest);

    
    //anybody
    public Task<IResult> SignUp(string courseId, string userId);

    public Task<IList<CoursePreviewResponse>> GetStudingCourses(string userId);
    public Task<IList<CoursePreviewResponse>> GetMyTeachingCourses(string userId);
    public Task<CourseDetailedResponse> GetCourseDetailedInfo(string courseId, string userId);

    public Task<IEnumerable<CoursePreviewResponse>> GetCourses(SortType? sort, string? search,
        bool? hasPlacesAndOpen, Semester? semester, int page, int pageSize);
    
    public bool IsUserMainTeacher(string courseId, string userId);
    public bool IsUserTeacher(string courseId, string userId);
}