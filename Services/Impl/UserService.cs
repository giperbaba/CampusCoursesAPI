using Microsoft.EntityFrameworkCore;
using repassAPI.Data;
using repassAPI.Models.Response;
using repassAPI.Services.Interfaces;

namespace repassAPI.Services.Impl;

public class UserService: IUserService
{
    private readonly DatabaseContext _context;

    public UserService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserShortResponse>> GetUsers()
    {
        var users = _context.Users.ToList();
        var usersResponse = users.Select(user => 
            new UserShortResponse(user.Id.ToString(), user.FullName));

        return usersResponse;
    }

    public async Task<UserRolesResponse> GetUserRoles(string id)
    {
        var userRoles = await _context.UserRoles
            .Include(ur => ur.User)
            .FirstOrDefaultAsync(ur => ur.UserId == Guid.Parse(id));
        
        return new UserRolesResponse(userRoles.IsTeacher, userRoles.IsStudent, userRoles.User.IsAdmin);
    }
}