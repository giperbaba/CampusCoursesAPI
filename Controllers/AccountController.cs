using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using repassAPI.Models.Request;
using repassAPI.Services.Interfaces;

namespace repassAPI.Controllers;

[ApiController]
public class AccountController: BaseController
{
    private readonly IAccountService _accountService;
    
    public AccountController(IAccountService accountService, IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _accountService = accountService;
    }

    [HttpPost("registration")]
    public async Task<IActionResult> Regist(UserRegisterRequest input)
    {
        return Ok(await _accountService.Register(input));
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginRequest input)
    {
        return Ok(await _accountService.Login(input));
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        return Ok(await _accountService.Logout(GetUserData(ClaimTypes.Email), GetAccessToken()));
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        return Ok(await _accountService.GetProfile(GetUserData(ClaimTypes.Email), GetAccessToken()));
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> EditProfile(UserEditProfileRequest input)
    {
        return Ok(await _accountService.EditProfile(GetUserData(ClaimTypes.Email), input, GetAccessToken()));
    }
}