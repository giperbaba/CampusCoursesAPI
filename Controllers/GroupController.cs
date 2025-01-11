using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using repassAPI.Models.Request;
using repassAPI.Services.Interfaces;

namespace repassAPI.Controllers;

public class GroupController: BaseController
{
    private readonly IGroupService _groupService;
    private readonly IUserService _userService;

    public GroupController(IGroupService groupService, IUserService userService) : base(userService)
    {
        _groupService = groupService;
        _userService = userService;
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
        await EnsureAdminRights(GetUserEmail(ClaimTypes.Name));
        return Ok(await _groupService.Create(input));
    }
    
    [Authorize]
    [HttpPut("groups/{id}")]
    public async Task<IActionResult> EditGroup(string id, CampusGroupEditRequest input)
    {
        await EnsureAdminRights(GetUserEmail(ClaimTypes.Name));
        return Ok(await _groupService.Edit(id, input));
    }
    
    [Authorize]
    [HttpDelete("groups/{id}")]
    public async Task<IActionResult> DeleteGroup(string id)
    {
        await EnsureAdminRights(GetUserEmail(ClaimTypes.Name));
        return Ok(await _groupService.Delete(id));
    }
    
    /*[Authorize]
    [HttpGet("groups/{id}")]
    public async Task<IActionResult> GetCourses(string id)
    {
        return Ok(await _groupService.GetCourses(id));
    }*/
    
}