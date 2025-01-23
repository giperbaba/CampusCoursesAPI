using repassAPI.Entities;
using repassAPI.Models.Enums;
using repassAPI.Models.Request;
using repassAPI.Models.Response;

namespace repassAPI.Utils;

public static class Mapper
{
    //User
    public static User MapUserFromRegisterModelToEntity(UserRegisterRequest userRequest)
    {
        var hashedPassword = PasswordHasher.Hash(userRequest.Password);

        return new User(userRequest.FullName, userRequest.BirthDate, userRequest.Email, hashedPassword);
    }

    public static UserProfileResponse MapUserEntityToUserProfileModel(User userEntity)
    {
        return new UserProfileResponse(userEntity.FullName, userEntity.Email, userEntity.BirthDate);
    }
    
    //Group
    public static CampusGroup MapGroupFromCreateModelToEntity(CampusGroupCreateRequest groupRequest)
    {
        return new CampusGroup(groupRequest.Name);
    }

    public static CampusGroupResponse MapGroupEntityToGroupModel(CampusGroup groupEntity)
    {
        return new CampusGroupResponse(groupEntity.Id.ToString(), groupEntity.Name);
    }
    
    //Course
    public static Course MapCourseFromCreateModelToEntity(Guid groupId, CourseCreateRequest courseRequest, int remainingSlots, CourseStatus courseStatus, Guid mainTeacherId)
    {
        return new Course(courseRequest.Name, courseRequest.StartYear, courseRequest.MaxStudentsCount,
            remainingSlots, courseStatus, courseRequest.Semester, courseRequest.Requirements, courseRequest.Annotations, DateTime.UtcNow, groupId, mainTeacherId);
    }

    public static CoursePreviewResponse MapCourseEntityToCoursePreviewModel(Course courseEntity)
    {
        return new CoursePreviewResponse(courseEntity.Id.ToString(), courseEntity.Name, courseEntity.StartYear,
            courseEntity.MaxStudentsCount, (courseEntity.MaxStudentsCount -
                courseEntity.StudentsEnrolledCount), courseEntity.Status,
            courseEntity.Semester);
    }

    public static CourseDetailedResponse MapCourseEntityToCourseDetailsModel(Course courseEntity, 
        IEnumerable<CourseStudentResponse> students, 
        IEnumerable<CourseTeacherResponse> teachers, IEnumerable<CourseNotificationResponse> notifications)
    {
        return new CourseDetailedResponse(courseEntity.Id.ToString(), courseEntity.Name, courseEntity.StartYear,
            courseEntity.MaxStudentsCount, courseEntity.StudentsEnrolledCount, courseEntity.StudentsInQueueCount,
            courseEntity.Requirements, courseEntity.Annotations, courseEntity.Status, courseEntity.Semester, students,
            teachers, notifications);
    }
    
    //Student
    
    //with results
    public static CourseStudentResponse MapStudentEntityToStudentModelWithResults(CourseStudent studentEntity)
    {
        return new CourseStudentResponse(studentEntity.StudentId, studentEntity.Student.FullName, studentEntity.Student.Email,
            studentEntity.Status, studentEntity.MidtermResult, studentEntity.FinalResult);
    }
    
    //without results 
    public static CourseStudentResponse MapStudentEntityToStudentModelWithoutResults(CourseStudent studentEntity)
    {
        return new CourseStudentResponse(studentEntity.StudentId, studentEntity.Student.FullName, studentEntity.Student.Email,
            studentEntity.Status, null, null);
    }
    
    //Teacher
    public static CourseTeacherResponse MapTeacherEntityToTeacherModel(CourseTeacher teacherEntity)
    {
        return new CourseTeacherResponse(teacherEntity.Teacher.FullName, teacherEntity.Teacher.Email, teacherEntity.IsMainTeacher);
    }
    
    //Notification
    public static CourseNotificationResponse MapNotificationEntityToNotificationModel(Notification notifEntity)
    {
        return new CourseNotificationResponse(notifEntity.Text, notifEntity.IsImportant);
    }

    public static Notification MapNotificationCreateModelToEntity(string courseId, NotificationCreateRequest notificationRequest)
    {
        return new Notification(Guid.Parse(courseId), notificationRequest.Text, notificationRequest.IsImportant);
    }
    
    //Report
    public static CampusGroupReportResponse MapCampusGroupEntityToReportResponse(CampusGroup groupEntity, int passedCount, int failedCount, int totalCount)
    {
        return new CampusGroupReportResponse
        {
            Id = groupEntity.Id,
            Name = groupEntity.Name,
            AveragePassed = totalCount == 0 ? 0 : passedCount / (double)totalCount,
            AverageFailed = totalCount == 0 ? 0 : failedCount / (double)totalCount
        };
    }
    
    public static TeacherReportRecordResponse MapTeacherToReportRecordResponse(
        User teacher,
        IEnumerable<CampusGroupReportResponse> groupResponses)
    {
        return new TeacherReportRecordResponse(teacher.FullName, teacher.Id, groupResponses.ToList());
    }
}
