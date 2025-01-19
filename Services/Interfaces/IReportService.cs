using repassAPI.Models.Enums;
using repassAPI.Models.Response;

namespace repassAPI.Services.Interfaces;

public interface IReportService
{
    public Task<IEnumerable<TeacherReportRecordResponse>> GetReport(Semester? semester, IList<Guid>? campusGroupIds);
}