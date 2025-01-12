using repassAPI.Models.Request;
using repassAPI.Models.Response;

namespace repassAPI.Services.Interfaces;

public interface IGroupService
{
    public Task<IEnumerable<CampusGroupResponse>> GetGroups();
    public Task<CampusGroupResponse> Create(CampusGroupCreateRequest groupRequest);
    public Task<CampusGroupResponse> Edit(string groupId, CampusGroupEditRequest groupRequest);
    public Task<IResult> Delete(string groupId);
    public Task<IEnumerable<CoursePreviewResponse>> GetCourses(string groupId);
}