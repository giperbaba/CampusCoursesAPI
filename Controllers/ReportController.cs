using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using repassAPI.Models.Enums;
using repassAPI.Services.Interfaces;

namespace repassAPI.Controllers;

[ApiController]
public class ReportController: BaseController
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService, IServiceProvider serviceProvider): base(serviceProvider)
    {
        _reportService = reportService;
    }

    [Authorize]
    [HttpGet("report")]
    public async Task<IActionResult> GetReport([FromQuery]Semester semester, [FromQuery]IList<Guid> campusGroupIds)
    {
        await EnsureAdminRights(GetUserData(ClaimTypes.Name));
        return Ok(await _reportService.GetReport(semester, campusGroupIds));
    }
}