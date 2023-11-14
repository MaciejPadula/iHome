using iHome.Features.SchedulesList.Api.Request;
using iHome.Shared.Controllers;
using iHome.Shared.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Features.SchedulesList.Api;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ScheduleController : BaseApiController
{
    private readonly ISchedulesListService _schedulesListService;

    public ScheduleController(ISchedulesListService schedulesListService, IUserAccessor userAccessor)
        : base(userAccessor)
    {
        _schedulesListService = schedulesListService;
    }

    [HttpGet("GetSchedules")]
    public async Task<IActionResult> GetSchedules()
    {
        var schedules = await _schedulesListService.GetUserSchedulesOrdered(_userAccessor.UserId);
        return Ok(schedules);
    }

    [HttpPost("UpdateSchedule")]
    public async Task<IActionResult> UpdateSchedule([FromBody] UpdateScheduleRequest request)
    {
        await _schedulesListService.UpdateScheduleTime(
            request.ScheduleId, request.ScheduleTime, request.Day, _userAccessor.UserId);
        return Ok();
    }

    [HttpDelete("RemoveSchedule/{id}")]
    public async Task<IActionResult> RemoveSchedule(Guid id)
    {
        await _schedulesListService.RemoveSchedule(id, _userAccessor.UserId);
        return Ok();
    }
}
