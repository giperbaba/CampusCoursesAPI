using Microsoft.AspNetCore.Mvc;

namespace repassAPI.Controllers;

public class BaseController: ControllerBase
{
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