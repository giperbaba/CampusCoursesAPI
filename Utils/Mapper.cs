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
            remainingSlots, courseStatus, courseRequest.Semester, courseRequest.Requirements, courseRequest.Annotations, groupId, mainTeacherId);
    }

    public static CoursePreviewResponse MapCourseEntityToCoursePreviewModel(Course courseEntity)
    {
        return new CoursePreviewResponse(courseEntity.Id.ToString(), courseEntity.Name, courseEntity.StartYear,
            courseEntity.MaxStudentsCount, courseEntity.RemainingSlotsCount, courseEntity.Status,
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
    public static CourseStudentResponse MapStudentEntityToStudentModel(CourseStudent studentEntity)
    {
        return new CourseStudentResponse(studentEntity.Id, studentEntity.Name, studentEntity.Email,
            studentEntity.Status, studentEntity.MidtermResult, studentEntity.FinalResult);
    }
    
    //Teacher
    public static CourseTeacherResponse MapTeacherEntityToTeacherModel(CourseTeacher teacherEntity)
    {
        return new CourseTeacherResponse(teacherEntity.Name, teacherEntity.Email, teacherEntity.IsMainTeacher);
    }
    
    //Notification
    public static CourseNotificationResponse MapNotificationEntityToNotificationModel(Notification notifEntity)
    {
        return new CourseNotificationResponse(notifEntity.Text, notifEntity.IsImportant);
    }
}
