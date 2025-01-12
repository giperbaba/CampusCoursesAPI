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
    public static Course MapCourseFromCreateModelToEntity(Guid groupId, CourseCreateRequest courseRequest, int remainingSlots, CourseStatus courseStatus)
    {
        return new Course(courseRequest.Name, courseRequest.StartYear, courseRequest.MaxStudentsCount,
            remainingSlots, courseStatus, courseRequest.Semester, groupId);
    }

    public static CoursePreviewResponse MapCourseEntityToCourseModel(Course courseEntity)
    {
        return new CoursePreviewResponse(courseEntity.Id.ToString(), courseEntity.Name, courseEntity.StartYear,
            courseEntity.MaxStudentsCount, courseEntity.RemainingSlotsCount, courseEntity.Status,
            courseEntity.Semester);
    }
}
