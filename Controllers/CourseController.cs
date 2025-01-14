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
        await EnsureAdminRights(GetUserData(ClaimTypes.Email));
        return Ok(await _courseService.CreateCourse(groupId, input));
    }
    
    [Authorize]
    [HttpPut("courses/{id}")]
    public async Task<IActionResult> EditCourse(string id, CourseEditRequest input)
    {
        await EnsureAdminRights(GetUserData(ClaimTypes.Email));
        return Ok(await _courseService.EditCourse(id, input));
    }
    
    [Authorize]
    [HttpDelete("courses/{id}")]
    public async Task<IActionResult> DeleteCourse(string id)
    {
        await EnsureAdminRights(GetUserData(ClaimTypes.Email));
        return Ok(await _courseService.DeleteCourse(id));
    }

    [Authorize]
    [HttpPost("courses/{id}/teachers")]
    public async Task<IActionResult> AddTeacherToCourse(string id, CourseAddTeacherRequest userId)
    {
        await EnsureMainRights(id, GetUserData(ClaimTypes.Name), GetUserData(ClaimTypes.Email));
        return Ok(await _courseService.AddTeacherToCourse(id, userId));
    }

    [Authorize]
    [HttpPost("courses/{id}/status")]
    public async Task<IActionResult> EditCourseStatus(string id, CourseEditStatusRequest input)
    {
        await EnsureAdminOrTeacherRights(id, GetUserData(ClaimTypes.Name), GetUserData(ClaimTypes.Email));
        return Ok(await _courseService.EditCourseStatus(id, input));
    }

    [Authorize]
    [HttpPut("courses/{id}/requirements-and-annotations")]
    public async Task<IActionResult> EditCourseReqAndAnnotations(string id, CourseEditReqAndAnnotationsRequest input)
    {
        await EnsureAdminOrTeacherRights(id, GetUserData(ClaimTypes.Name), GetUserData(ClaimTypes.Email));
        return Ok(await _courseService.EditCourseReqAndAnnotations(id, input));
    }

    [Authorize]
    [HttpPost("courses/{id}/sign-up")]
    public async Task<IActionResult> SignUp(string id)
    {
        return Ok(await _courseService.SignUp(id, GetUserData(ClaimTypes.Name)));
    }
    
    [Authorize]
    [HttpGet("courses/{id}/details")]
    public async Task<IActionResult> GetDetailedInfo(string id)
    {
        return Ok(await _courseService.GetCourseDetailedInfo(id));
    }
}