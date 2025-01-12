using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using repassAPI.Models.Request;
using repassAPI.Services.Interfaces;

namespace repassAPI.Controllers;

[ApiController]
public class CourseController: BaseController
{
    private readonly ICourseService _courseService;

    public CourseController(ICourseService courseService, IServiceProvider serviceProvider): base(serviceProvider)
    {
        _courseService = courseService;
    }

    [Authorize]
    [HttpPost("groups/{groupId}")]
    public async Task<IActionResult> CreateCourse(string groupId, CourseCreateRequest input)
    {
        await EnsureAdminRights(GetUserEmail(ClaimTypes.Name));
        return Ok(await _courseService.CreateCourse(groupId, input));
    }
}