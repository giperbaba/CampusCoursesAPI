using Microsoft.AspNetCore.Mvc;
using repassAPI.Exceptions;
using repassAPI.Services.Interfaces;

namespace repassAPI.Controllers;

public class BaseController : ControllerBase
{
    private readonly Lazy<IUserService> _userService;

    protected BaseController(IServiceProvider serviceProvider)
    {
        // Используем Lazy для отложенной загрузки зависимости
        _userService = new Lazy<IUserService>(() => serviceProvider.GetService<IUserService>());
    }

    protected async Task EnsureAdminRights(string email)
    {
        var isAdmin = _userService.Value.IsUserAdmin(email);
        if (!isAdmin)
        {
            throw new AccessDeniedException(Constants.ErrorMessages.AccessDenied);
        }
    }
    protected string GetUserEmail(string claimType)
    {
        var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;

        if (email == null) throw new UnauthorizedAccessException();

        return email;
    }
    
    protected string GetAccessToken()
    {
        var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            throw new UnauthorizedAccessException("Authorization token is missing or invalid.");
        }

        var token = authorizationHeader.Substring("Bearer ".Length);
        return token;
    }
}