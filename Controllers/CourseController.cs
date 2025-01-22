using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using repassAPI.Models.Enums;
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
    public async Task<IActionResult> CreateCourse(Guid groupId, CourseCreateRequest input)
    {
        await EnsureAdminRights(GetUserData(ClaimTypes.Name));
        return Ok(await _courseService.CreateCourse(groupId, input));
    }
    
    [Authorize]
    [HttpDelete("courses/{id}")]
    public async Task<IActionResult> DeleteCourse(string id)
    {
        await EnsureAdminRights(GetUserData(ClaimTypes.Name));
        return Ok(await _courseService.DeleteCourse(id));
    }
    
    [Authorize]
    [HttpPut("courses/{id}")]
    public async Task<IActionResult> EditCourse(string id, CourseEditRequest input)
    {
        await EnsureAdminRights(GetUserData(ClaimTypes.Name));
        return Ok(await _courseService.EditCourse(id, input));
    }
    
    
    [Authorize]
    [HttpPut("courses/{id}/requirements-and-annotations")]
    public async Task<IActionResult> EditCourseReqAndAnnotations(string id, CourseEditReqAndAnnotationsRequest input)
    {
        await EnsureAdminOrTeacherRights(id, GetUserData(ClaimTypes.Name));
        return Ok(await _courseService.EditCourseReqAndAnnotations(id, input));
    }
    
    [Authorize]
    [HttpPost("courses/{id}/status")]
    public async Task<IActionResult> EditCourseStatus(string id, CourseEditStatusRequest input)
    {
        await EnsureAdminOrTeacherRights(id, GetUserData(ClaimTypes.Name));
        return Ok(await _courseService.EditCourseStatus(id, input));
    }
    
    [Authorize]
    [HttpPost("courses/{id}/sign-up")]
    public async Task<IActionResult> SignUp(string id)
    {
        return Ok(await _courseService.SignUp(id, GetUserData(ClaimTypes.Name)));
    }
    
    [Authorize]
    [HttpPost("courses/{id}/student-status/{studentId}")]
    public async Task<IActionResult> EditStudentStatus(string id, string studentId, CourseStudentEditStatusRequest input)
    {
        await EnsureAdminOrTeacherRights(id, GetUserData(ClaimTypes.Name));
        return Ok(await _courseService.EditStudentStatus(id, studentId, input));
    }

    [Authorize]
    [HttpPost("courses/{id}/marks/{studentId}")]
    public async Task<IActionResult> EditStudentMark(string id, string studentId, CourseStudentEditMarkRequest input)
    {
        await EnsureAdminOrTeacherRights(id, GetUserData(ClaimTypes.Name));
        return Ok(await _courseService.EditStudentMark(id, studentId, input));
    }
    
    [Authorize]
    [HttpPost("courses/{id}/notifications")]
    public async Task<IActionResult> CreateNotification(string id, NotificationCreateRequest input)
    {
        await EnsureAdminOrTeacherRights(id, GetUserData(ClaimTypes.Name));
        return Ok(await _courseService.CreateNewNotification(id, input));
    }

    [Authorize]
    [HttpPost("courses/{id}/teachers")]
    public async Task<IActionResult> AddTeacherToCourse(string id, CourseAddTeacherRequest userId)
    {
        await EnsureMainRights(id, GetUserData(ClaimTypes.Name));
        return Ok(await _courseService.AddTeacherToCourse(id, userId));
    }
    
    [Authorize]
    [HttpGet("courses/{id}/details")]
    public async Task<IActionResult> GetDetailedInfo(string id)
    {
        return Ok(await _courseService.GetCourseDetailedInfo(id, GetUserData(ClaimTypes.Name)));
    }

    [Authorize]
    [HttpGet("courses/my")]
    public async Task<IActionResult> GetMyCourses()
    {
        return Ok(await _courseService.GetStudingCourses(GetUserData(ClaimTypes.Name)));
    }
    
    [Authorize]
    [HttpGet("courses/teaching")]
    public async Task<IActionResult> GetTeachingCourses()
    {
        return Ok(await _courseService.GetMyTeachingCourses(GetUserData(ClaimTypes.Name)));
    }
    
    [HttpGet("courses/list")]
    public async Task<IActionResult> GetCourses([FromQuery] SortType? sort,
        [FromQuery] string? search,
        [FromQuery] bool? hasPlacesAndOpen,
        [FromQuery] Semester? semester,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var courses = await _courseService.GetCourses(sort, search, hasPlacesAndOpen, 
            semester, page, pageSize);

        return Ok(courses);
    }

}