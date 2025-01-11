using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using repassAPI.Models.Request;
using repassAPI.Services.Interfaces;

namespace repassAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController: BaseController
{
    private readonly IUserService _userService;
    
    public AccountController(IUserService userService): base(userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterRequest input)
    {
        return Ok(await _userService.Register(input));
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginRequest input)
    {
        return Ok(await _userService.Login(input));
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        return Ok(await _userService.Logout(GetUserEmail(ClaimTypes.Name), GetAccessToken()));
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        return Ok(await _userService.GetProfile(GetUserEmail(ClaimTypes.Name), GetAccessToken()));
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> EditProfile(UserEditProfileRequest input)
    {
        return Ok(await _userService.EditProfile(GetUserEmail(ClaimTypes.Name), input, GetAccessToken()));
    }
}