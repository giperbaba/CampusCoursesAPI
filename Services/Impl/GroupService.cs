using Microsoft.EntityFrameworkCore;
using repassAPI.Data;
using repassAPI.Entities;
using repassAPI.Exceptions;
using repassAPI.Models.Request;
using repassAPI.Models.Response;
using repassAPI.Services.Interfaces;
using repassAPI.Utils;

namespace repassAPI.Services.Impl;

public class GroupService: IGroupService
{
    private readonly DatabaseContext _context;

    public GroupService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CampusGroupResponse>> GetGroups()
    {
        var groups = await _context.CampusGroups.ToListAsync();
        return groups.Select(Mapper.MapGroupEntityToGroupModel);
    }

    public async Task<CampusGroupResponse> Create(CampusGroupCreateRequest groupRequest)
    {
        var group = Mapper.MapGroupFromCreateModelToEntity(groupRequest);
        await _context.CampusGroups.AddAsync(group);
        await _context.SaveChangesAsync();

        var groupResponse = Mapper.MapGroupEntityToGroupModel(group);
        return groupResponse;
    }
    
    public async Task<CampusGroupResponse> Edit(string groupId, CampusGroupEditRequest groupRequest)
    {
        var group = await GetGroupById(groupId);

        group.Name = groupRequest.Name;

        _context.CampusGroups.Update(group);
        await _context.SaveChangesAsync();

        var groupResponse = Mapper.MapGroupEntityToGroupModel(group);
        return groupResponse;
    }
    
    public async Task<IResult> Delete(string groupId)
    {
        var group = await GetGroupById(groupId);

        _context.CampusGroups.Remove(group);
        await _context.SaveChangesAsync();

        return Results.Ok();
    }
    
    public async Task<IEnumerable<CoursePreviewResponse>> GetCourses(string groupId)
    {
        var group = await _context.CampusGroups
            .Include(g => g.Courses)
            .ThenInclude(c => c.Students)
            .FirstOrDefaultAsync(g => g.Id.ToString() == groupId);

        if (group == null)
        {
            throw new NotFoundException(Constants.ErrorMessages.GroupNotFound);
        }

        return group.Courses.Select(Mapper.MapCourseEntityToCoursePreviewModel);
    }

    private async Task<CampusGroup> GetGroupById(string id)
    {
        var group = _context.CampusGroups.FirstOrDefault(g => g.Id.ToString() == id);
        if (group == null)
        {
            throw new NotFoundException(Constants.ErrorMessages.GroupNotFound);
        }

        return group;
    }
}