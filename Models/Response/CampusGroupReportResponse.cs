namespace repassAPI.Models.Response;

public class CampusGroupReportResponse
{
    public string Name { get; set; }
    public Guid Id { get; set; }
    public double AveragePassed { get; set; }
    public double AverageFailed { get; set; }
}