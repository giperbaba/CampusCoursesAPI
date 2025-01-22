using repassAPI.Models.Enums;
using repassAPI.Models.Request;
using repassAPI.Models.Response;

namespace repassAPI.Services.Interfaces;

public interface ICourseService
{
    //admin
    public Task<CoursePreviewResponse> CreateCourse(Guid groupId, CourseCreateRequest request);
    public Task<CourseDetailedResponse> EditCourse(Guid courseId, CourseEditRequest request);
    public Task<CoursePreviewResponse> DeleteCourse(Guid id);
    
    //admin, main teacher
    public Task<CourseDetailedResponse> AddTeacherToCourse(string courseId, CourseAddTeacherRequest request);
    
    //admin, teacher
    public Task<CourseDetailedResponse> CreateNewNotification(string courseId,
        NotificationCreateRequest notificationCreateRequest);

    public Task<CourseDetailedResponse> EditCourseStatus(Guid courseId, CourseEditStatusRequest request);
    public Task<CourseDetailedResponse> EditCourseReqAndAnnotations(Guid courseId,
        CourseEditReqAndAnnotationsRequest request);
    public Task<CourseDetailedResponse> EditStudentMark(Guid courseId, Guid studentId,
        CourseStudentEditMarkRequest editMarkRequest);
    public Task<CourseDetailedResponse> EditStudentStatus(Guid courseId, Guid studentId,
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