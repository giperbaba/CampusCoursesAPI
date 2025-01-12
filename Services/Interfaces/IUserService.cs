using repassAPI.Models.Response;

namespace repassAPI.Services.Interfaces;

public interface IUserService
{ 
    public Task<IEnumerable<UserShortResponse>> GetUsers();
    public Task<UserRolesResponse> GetUserRoles(string id);
}