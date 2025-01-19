namespace repassAPI.Models.Response;

public class TeacherReportRecordResponse(
    string fullName,
    Guid id,
    IEnumerable<CampusGroupReportResponse> campusGroupReports)
{
    public string FullName { get; set; } = fullName;
    public Guid Id { get; set; } = id; //TODO: Base сущность с Id
    public IEnumerable<CampusGroupReportResponse> CampusGroupReports { get; set; } = campusGroupReports;
}