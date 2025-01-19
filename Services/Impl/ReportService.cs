using Microsoft.EntityFrameworkCore;
using repassAPI.Constants;
using repassAPI.Data;
using repassAPI.Exceptions;
using repassAPI.Models.Enums;
using repassAPI.Models.Response;
using repassAPI.Services.Interfaces;
using repassAPI.Utils;

namespace repassAPI.Services.Impl;

public class ReportService: IReportService
{
    private readonly DatabaseContext _context;

    public ReportService(DatabaseContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<TeacherReportRecordResponse>> GetReport(Semester? semester, IList<Guid>? campusGroupIds)
    {
        if (campusGroupIds != null && campusGroupIds.Any())
        {
            if (!await AreCampusGroupIdsValid(campusGroupIds))
            {
                throw new NotFoundException(ErrorMessages.InvalidGroupsId);
            }
        }
        
        var query = _context.Courses
            .Include(c => c.CampusGroup)
            .Include(c => c.Students)
            .Include(c => c.MainTeacher)
            .AsQueryable();

        if (semester.HasValue)
        {
            query = query.Where(c => c.Semester == semester);
        }

        if (campusGroupIds != null && campusGroupIds.Any())
        {
            query = query.Where(c => campusGroupIds.Contains(c.CampusGroupId));
        }

        var groupedData = await query
            .GroupBy(c => c.MainTeacher)
            .ToListAsync();
        
        var report = groupedData.Select(group =>
        {
            var groupResponses = group
                .GroupBy(c => c.CampusGroup)
                .Select(g =>
                {
                    var totalStudents = g.Sum(c => c.Students.Count);
                    var passedCount = g.SelectMany(c => c.Students).Count(s => s.FinalResult == StudentMark.Passed);
                    var failedCount = g.SelectMany(c => c.Students).Count(s => s.FinalResult == StudentMark.Failed);

                    return Mapper.MapCampusGroupEntityToReportResponse(g.Key, passedCount, failedCount, totalStudents);
                });

            return Mapper.MapTeacherToReportRecordResponse(group.Key, groupResponses);
        }).ToList();

        return report;
    }
    
    private async Task<bool> AreCampusGroupIdsValid(IList<Guid> campusGroupIds)
    {
        if (campusGroupIds == null || !campusGroupIds.Any())
            return true;

        var existingGroupIds = await _context.CampusGroups
            .Where(cg => campusGroupIds.Contains(cg.Id))
            .Select(cg => cg.Id)
            .ToListAsync();
        
        return !campusGroupIds.Except(existingGroupIds).Any();
    }
}