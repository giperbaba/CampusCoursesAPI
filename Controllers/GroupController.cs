using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using repassAPI.Models.Request;
using repassAPI.Services.Interfaces;

namespace repassAPI.Controllers;

[ApiController]
public class GroupController: BaseController
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService, IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _groupService = groupService;
    }

    [Authorize]
    [HttpGet("groups")]
    public async Task<IActionResult> GetGroups()
    {
        return Ok(await _groupService.GetGroups());
    }

    [Authorize]
    [HttpPost("groups")]
    public async Task<IActionResult> CreateGroup(CampusGroupCreateRequest input)
    {
        await EnsureAdminRights(GetUserData(ClaimTypes.Name));
        return Ok(await _groupService.Create(input));
    }
    
    [Authorize]
    [HttpPut("groups/{id}")]
    public async Task<IActionResult> EditGroup(Guid id, CampusGroupEditRequest input)
    {
        await EnsureAdminRights(GetUserData(ClaimTypes.Name));
        return Ok(await _groupService.Edit(id.ToString(), input));
    }
    
    [Authorize]
    [HttpDelete("groups/{id}")]
    public async Task<IActionResult> DeleteGroup(Guid id)
    {
        await EnsureAdminRights(GetUserData(ClaimTypes.Name));
        return Ok(await _groupService.Delete(id.ToString()));
    }
    
    [Authorize]
    [HttpGet("groups/{id}")]
    public async Task<IActionResult> GetCourses(Guid id)
    {
        return Ok(await _groupService.GetCourses(id.ToString()));
    }
}