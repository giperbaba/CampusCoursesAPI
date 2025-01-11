using repassAPI.Entities;
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
}
