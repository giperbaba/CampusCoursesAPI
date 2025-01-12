using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using repassAPI.Models.Request;
using repassAPI.Services.Interfaces;

namespace repassAPI.Controllers;

[ApiController]
public class TokenController: BaseController
{
    private readonly IAccountService _accountService;
    
    public TokenController(IAccountService accountService, IServiceProvider serviceProvider): base(serviceProvider)
    {
        _accountService = accountService;
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshRequest refreshRequest)
    {
        return Ok(await _accountService.Refresh(refreshRequest));
    }
}