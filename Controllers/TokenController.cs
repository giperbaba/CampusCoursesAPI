using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using repassAPI.Models.Request;
using repassAPI.Services.Interfaces;

namespace repassAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TokenController: BaseController
{
    private readonly IUserService _userService;
    
    public TokenController(IUserService userService): base(userService)
    {
        _userService = userService;
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshRequest refreshRequest)
    {
        return Ok(await _userService.Refresh(refreshRequest));
    }
}