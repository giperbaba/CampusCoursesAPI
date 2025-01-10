using repassAPI.Entities;
using repassAPI.Models.Request;
using repassAPI.Models.Response;

namespace repassAPI.Utils;

public static class Mapper
{
    public static User MapUserFromRegisterModelToEntity(UserRegisterRequest userRequest)
    {
        var hashedPassword = PasswordHasher.Hash(userRequest.Password);

        return new User(userRequest.FullName, userRequest.BirthDate, userRequest.Email, hashedPassword);
    }

    public static UserProfileResponse MapUserEntityToUserProfileModel(User userEntity)
    {
        return new UserProfileResponse(userEntity.FullName, userEntity.Email, userEntity.BirthDate);
    }
}
