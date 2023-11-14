using iHome.Features.AddSchedule.Api.Request;
using iHome.Features.AddSchedule.Api.Response;
using iHome.Shared.Controllers;
using iHome.Shared.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Features.AddSchedule.Api;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AddScheduleController : BaseApiController
{
    private readonly IAddScheduleService _addScheduleService;

    public AddScheduleController(IAddScheduleService addScheduleService, IUserAccessor userAccessor)
        : base(userAccessor)
    {
        _addScheduleService = addScheduleService;
    }

    [HttpPost]
    public async Task<IActionResult> AddSchedule([FromBody] AddScheduleRequest request)
    {
        await _addScheduleService.AddSchedule(
            request.ScheduleName, request.Day, request.ScheduleTime, _userAccessor.UserId);
        return Ok();
    }

    [HttpPost("GetSuggestedHour")]
    public async Task<IActionResult> GetSuggestedHour(GetSuggestedHourRequest request)
    {
        var time = await _addScheduleService.GetSuggestedTime(request.ScheduleName);

        return Ok(new GetSuggestedHourResponse
        {
            Hour = time.Hour,
            Minute = time.Minute
        });
    }

    [HttpPost("GetSuggestedDevices")]
    public async Task<IActionResult> GetSuggestedDevices(GetSuggestedDevicesRequest request)
    {
        var devices = await _addScheduleService.GetSuggestedDevices(
            request.ScheduleName, request.ScheduleTime, _userAccessor.UserId);

        return Ok(devices);
    }
}
