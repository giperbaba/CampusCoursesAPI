using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using repassAPI.Services.Interfaces;

namespace repassAPI.Controllers;

[ApiController]
public class UsersController: BaseController
{
    private readonly IUserService _userService;
    
    public UsersController(IServiceProvider serviceProvider, IUserService userService): base(serviceProvider)
    {
        _userService = userService;
    }
    
    [Authorize]
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        await EnsureAdminRights(GetUserData(ClaimTypes.Name));
        return Ok(await _userService.GetUsers());
    }

    [Authorize]
    [HttpGet("roles")]
    public async Task<IActionResult> GetUserRoles()
    {
        return Ok(await _userService.GetUserRoles(GetUserData(ClaimTypes.Name)));
    }
}