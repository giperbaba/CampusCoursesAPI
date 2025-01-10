using repassAPI.Models.Request;
using repassAPI.Models.Response;

namespace repassAPI.Services.Interfaces;

public interface IUserService
{
    public Task<TokensResponse> Register(UserRegisterRequest userRegister);
    public Task<TokensResponse> Login(UserLoginRequest userLogin);
    public Task<AccessTokenResponse> Refresh(RefreshRequest refreshToken);
    public Task<Response> Logout(string email, string accessToken);
    public Task<UserProfileResponse> GetProfile(string email, string accessToken);
    public Task<UserProfileResponse> EditProfile(string email, UserEditProfileRequest userEdit, string accessToken);
}