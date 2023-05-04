using iHome.Core.Services.Schedules;
using iHome.Logic;
using iHome.Models.Requests.Schedules;
using iHome.Models.Responses.Schedules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ScheduleController : ControllerBase
{
    private readonly IScheduleService _scheduleService;

    private readonly IUserAccessor _userAccessor;

    public ScheduleController(IScheduleService scheduleService, IUserAccessor userAccessor)
    {
        _scheduleService = scheduleService;
        _userAccessor = userAccessor;
    }

    [HttpPost("AddSchedule")]
    public async Task<IActionResult> AddSchedule([FromBody] AddScheduleRequest request)
    {
        await _scheduleService.AddSchedule(request.ScheduleName,
            request.Day, request.Hour, request.Minute,
            _userAccessor.UserId);

        return Ok();
    }

    [HttpGet("GetSchedules")]
    public async Task<IActionResult> GetSchedules()
    {
        var schedules = await _scheduleService.GetSchedules(_userAccessor.UserId);

        return Ok(schedules
            .OrderBy(s => s.Hour)
            .ThenBy(s => s.Minute));
    }

    [HttpGet("GetSchedule/{id}")]
    public async Task<IActionResult> GetSchedule(Guid id)
    {
        var schedule = await _scheduleService.GetScheduleWithDevices(id, _userAccessor.UserId);

        return Ok(schedule);
    }

    [HttpGet("GetScheduleDevicesCount")]
    public async Task<IActionResult> GetScheduleDevicesCount(Guid scheduleId)
    {
        return Ok(new GetScheduleDevicesCountResponse
        {
            Count = await _scheduleService.GetDevicesInScheduleCount(scheduleId, _userAccessor.UserId)
        });
    }

    [HttpPost("UpdateSchedule")]
    public async Task<IActionResult> UpdateSchedule([FromBody] UpdateScheduleRequest request)
    {
        await _scheduleService.UpdateScheduleTime(request.ScheduleId, request.Day, request.Hour, request.Minute, _userAccessor.UserId);

        return Ok();
    }

    [HttpGet("GetScheduleDevices/{id}")]
    public async Task<IActionResult> GetScheduleDevices(Guid id)
    {
        var scheduleDevices = await _scheduleService.GetScheduleDevices(id, _userAccessor.UserId);

        return Ok(scheduleDevices);
    }

    [HttpDelete("RemoveSchedule/{id}")]
    public async Task<IActionResult> RemoveSchedule(Guid id)
    {
        await _scheduleService.RemoveSchedule(id, _userAccessor.UserId);
        return Ok();
    }

    [HttpPost("AddOrUpdateScheduleDevice")]
    public async Task<IActionResult> AddOrUpdateScheduleDevice(AddOrUpdateScheduleDeviceRequest request)
    {
        await _scheduleService.AddOrUpdateDeviceSchedule(request.ScheduleId,
            request.DeviceId, request.DeviceData,
            _userAccessor.UserId);
        return Ok();
    }
}
