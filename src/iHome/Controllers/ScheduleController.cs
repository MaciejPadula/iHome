using iHome.Core.Services;
using iHome.Logic;
using iHome.Microservices.Schedules.Contract;
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
    private readonly IScheduleDeviceManagementService _scheduleDeviceManagementService;
    private readonly IScheduleManagementService _scheduleManagementService;

    private readonly IUserAccessor _userAccessor;

    public ScheduleController(IScheduleDeviceManagementService scheduleDeviceManagementService, IScheduleManagementService scheduleManagementService, IUserAccessor userAccessor)
    {
        _scheduleDeviceManagementService = scheduleDeviceManagementService;
        _scheduleManagementService = scheduleManagementService;
        _userAccessor = userAccessor;
    }

    [HttpPost("AddSchedule")]
    public async Task<IActionResult> AddSchedule([FromBody] AddScheduleRequest request)
    {
        await _scheduleManagementService.AddSchedule(new()
        {
            ScheduleName = request.ScheduleName,
            Day = request.Day,
            Hour = request.Hour,
            Minute = request.Minute,
            UserId = _userAccessor.UserId
        });

        return Ok();
    }

    [HttpGet("GetSchedules")]
    public async Task<IActionResult> GetSchedules()
    {
        var schedules = await _scheduleManagementService.GetSchedules(new()
        {
            UserId = _userAccessor.UserId
        });

        return Ok(schedules
            .Schedules
            .OrderBy(s => s.Hour)
            .ThenBy(s => s.Minute));
    }

    [HttpGet("GetSchedule/{id}")]
    public async Task<IActionResult> GetSchedule(Guid id)
    {
        var schedule = await _scheduleManagementService.GetSchedule(new()
        {
            ScheduleId = id,
            UserId = _userAccessor.UserId
        });

        return Ok(schedule.Schedule);
    }

    [HttpGet("GetScheduleDevicesCount")]
    public async Task<IActionResult> GetScheduleDevicesCount(Guid scheduleId)
    {
        var response = await _scheduleDeviceManagementService.GetDevicesInScheduleCount(new()
        {
            ScheduleId = scheduleId,
            UserId = _userAccessor.UserId
        });

        return Ok(new GetScheduleDevicesCountResponse
        {
            Count = response.NumberOfDevices
        });
    }

    [HttpPost("UpdateSchedule")]
    public async Task<IActionResult> UpdateSchedule([FromBody] UpdateScheduleRequest request)
    {
        await _scheduleManagementService.UpdateScheduleTime(new()
        {
            ScheduleId = request.ScheduleId,
            Day = request.Day,
            Hour = request.Hour,
            Minute = request.Minute,
            UserId = _userAccessor.UserId
        });

        return Ok();
    }

    [HttpGet("GetScheduleDevices/{id}")]
    public async Task<IActionResult> GetScheduleDevices(Guid id)
    {
        var scheduleDevices = await _scheduleDeviceManagementService.GetScheduleDevices(new()
        {
            ScheduleId = id,
            UserId = _userAccessor.UserId
        });

        return Ok(scheduleDevices.ScheduleDevices);
    }

    [HttpDelete("RemoveSchedule/{id}")]
    public async Task<IActionResult> RemoveSchedule(Guid id)
    {
        await _scheduleManagementService.RemoveSchedule(new()
        {
            ScheduleId = id,
            UserId = _userAccessor.UserId
        });
        return Ok();
    }

    [HttpPost("AddOrUpdateScheduleDevice")]
    public async Task<IActionResult> AddOrUpdateScheduleDevice(AddOrUpdateScheduleDeviceRequest request)
    {
        await _scheduleDeviceManagementService.AddOrUpdateDeviceSchedule(new()
        {
            ScheduleId = request.ScheduleId,
            DeviceId = request.DeviceId,
            DeviceData = request.DeviceData,
            UserId = _userAccessor.UserId
        });
        return Ok();
    }
}
