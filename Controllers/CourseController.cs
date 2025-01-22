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
    public async Task<IActionResult> DeleteCourse(Guid id)
    {
        await EnsureAdminRights(GetUserData(ClaimTypes.Name));
        return Ok(await _courseService.DeleteCourse(id));
    }
    
    [Authorize]
    [HttpPut("courses/{id}")]
    public async Task<IActionResult> EditCourse(Guid id, CourseEditRequest input)
    {
        await EnsureAdminRights(GetUserData(ClaimTypes.Name));
        return Ok(await _courseService.EditCourse(id, input));
    }
    
    
    [Authorize]
    [HttpPut("courses/{id}/requirements-and-annotations")]
    public async Task<IActionResult> EditCourseReqAndAnnotations(Guid id, CourseEditReqAndAnnotationsRequest input)
    {
        await EnsureAdminOrTeacherRights(id.ToString(), GetUserData(ClaimTypes.Name));
        return Ok(await _courseService.EditCourseReqAndAnnotations(id, input));
    }
    
    [Authorize]
    [HttpPost("courses/{id}/status")]
    public async Task<IActionResult> EditCourseStatus(Guid id, CourseEditStatusRequest input)
    {
        await EnsureAdminOrTeacherRights(id.ToString(), GetUserData(ClaimTypes.Name));
        return Ok(await _courseService.EditCourseStatus(id, input));
    }
    
    [Authorize]
    [HttpPost("courses/{id}/sign-up")]
    public async Task<IActionResult> SignUp(Guid id)
    {
        return Ok(await _courseService.SignUp(id.ToString(), GetUserData(ClaimTypes.Name)));
    }
    
    [Authorize]
    [HttpPost("courses/{id}/student-status/{studentId}")]
    public async Task<IActionResult> EditStudentStatus(Guid id, Guid studentId, CourseStudentEditStatusRequest input)
    {
        await EnsureAdminOrTeacherRights(id.ToString(), GetUserData(ClaimTypes.Name));
        return Ok(await _courseService.EditStudentStatus(id, studentId, input));
    }

    [Authorize]
    [HttpPost("courses/{id}/marks/{studentId}")]
    public async Task<IActionResult> EditStudentMark(Guid id, Guid studentId, CourseStudentEditMarkRequest input)
    {
        await EnsureAdminOrTeacherRights(id.ToString(), GetUserData(ClaimTypes.Name));
        return Ok(await _courseService.EditStudentMark(id, studentId, input));
    }
    
    [Authorize]
    [HttpPost("courses/{id}/notifications")]
    public async Task<IActionResult> CreateNotification(Guid id, NotificationCreateRequest input)
    {
        await EnsureAdminOrTeacherRights(id.ToString(), GetUserData(ClaimTypes.Name));
        return Ok(await _courseService.CreateNewNotification(id.ToString(), input));
    }

    [Authorize]
    [HttpPost("courses/{id}/teachers")]
    public async Task<IActionResult> AddTeacherToCourse(Guid id, CourseAddTeacherRequest userId)
    {
        await EnsureMainRights(id.ToString(), GetUserData(ClaimTypes.Name));
        return Ok(await _courseService.AddTeacherToCourse(id.ToString(), userId));
    }
    
    [Authorize]
    [HttpGet("courses/{id}/details")]
    public async Task<IActionResult> GetDetailedInfo(Guid id)
    {
        return Ok(await _courseService.GetCourseDetailedInfo(id.ToString(), GetUserData(ClaimTypes.Name)));
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