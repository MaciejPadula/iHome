using iHome.Core.Services;
using iHome.Logic;
using iHome.Models.Requests.Schedules;
using iHome.Models.Responses.Schedules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ScheduleController : BaseApiController
{
    private readonly IScheduleService _scheduleService;

    public ScheduleController(IScheduleService scheduleService, IUserAccessor userAccessor)
        : base(userAccessor)
    {
        _scheduleService = scheduleService;
    }

    [HttpPost("AddSchedule")]
    public async Task<IActionResult> AddSchedule([FromBody] AddScheduleRequest request)
    {
        await _scheduleService.AddSchedule(
            request.ScheduleName, request.Day, request.ScheduleTime, _userAccessor.UserId);
        return Ok();
    }

    [HttpGet("GetSchedules")]
    public async Task<IActionResult> GetSchedules()
    {
        var schedules = await _scheduleService.GetSchedulesOrdered(_userAccessor.UserId);
        return Ok(schedules);
    }

    [HttpGet("GetSchedule/{id}")]
    public async Task<IActionResult> GetSchedule(Guid id)
    {
        var schedule = await _scheduleService.GetSchedule(id, _userAccessor.UserId);
        return Ok(schedule);
    }

    [HttpPost("UpdateSchedule")]
    public async Task<IActionResult> UpdateSchedule([FromBody] UpdateScheduleRequest request)
    {
        await _scheduleService.UpdateScheduleTime(
            request.ScheduleId, request.Day, request.ScheduleTime, _userAccessor.UserId);
        return Ok();
    }

    [HttpGet("GetScheduleDevices/{id}")]
    public async Task<IActionResult> GetScheduleDevices(Guid id)
    {
        var devices = await _scheduleService.GetScheduleDevices(id, _userAccessor.UserId);
        return Ok(devices);
    }

    [HttpDelete("RemoveSchedule/{id}")]
    public async Task<IActionResult> RemoveSchedule(Guid id)
    {
        await _scheduleService.RemoveSchedule(id, _userAccessor.UserId);
        return Ok();
    }

    [HttpPost("AddOrUpdateScheduleDevice")]
    public async Task<IActionResult> AddOrUpdateScheduleDevice([FromBody] AddOrUpdateScheduleDeviceRequest request)
    {
        var id = await _scheduleService.AddOrUpdateScheduleDevice(
            request.ScheduleId, request.DeviceId, request.DeviceData, _userAccessor.UserId);
        return Ok(new AddOrUpdateScheduleDeviceResponse()
        {
            ScheduleDeviceId = id
        });
    }
}
