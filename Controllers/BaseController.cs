using Microsoft.AspNetCore.Mvc;
using repassAPI.Exceptions;
using repassAPI.Services.Interfaces;

namespace repassAPI.Controllers;

public class BaseController : ControllerBase
{
    private readonly Lazy<IAccountService> _accountService;
    private readonly Lazy<ICourseService> _courseService;

    protected BaseController(IServiceProvider serviceProvider)
    {
        //Lazy для отложенной загрузки зависимости, тк account service нужен в базовом, но не нужен в контроллерах, которые наследуются от базового
        _accountService = new Lazy<IAccountService>(() => serviceProvider.GetService<IAccountService>());
        _courseService = new Lazy<ICourseService>(() => serviceProvider.GetService<ICourseService>());
    }

    protected async Task EnsureAdminRights(string id)
    {
        var isAdmin = _accountService.Value.IsUserAdmin(id);
        if (!isAdmin)
        {
            throw new AccessDeniedException(Constants.ErrorMessages.AccessDenied);
        }
    }

    protected async Task EnsureMainRights(string courseId, string userId)
    {
        var isAdmin = _accountService.Value.IsUserAdmin(userId);
        var isMainTeacher = _courseService.Value.IsUserMainTeacher(courseId, userId);
        if (!isMainTeacher && !isAdmin)
        {
            throw new AccessDeniedException(Constants.ErrorMessages.AccessDeniedAdminMainTeacher);
        }
        
    }
    protected async Task EnsureAdminOrTeacherRights(string courseId, string userId)
    {
        var isAdmin = _accountService.Value.IsUserAdmin(userId);
        var isTeacher = _courseService.Value.IsUserTeacher(courseId, userId);
        
        if (!isAdmin && !isTeacher)
        {
            throw new AccessDeniedException(Constants.ErrorMessages.AccessDeniedAdminTeacher);
        }
    }
    protected string GetUserData(string claimType)
    {
        var data = HttpContext.User.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;

        if (data == null) throw new UnauthorizedAccessException();

        return data;
    }
    
    protected string GetAccessToken()
    {
        var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            throw new UnauthorizedAccessException("Authorization token is missing or invalid.");
        }

        var token = authorizationHeader.Substring("Bearer ".Length); //TODO: чо такое сабстринг
        return token;
    }
}