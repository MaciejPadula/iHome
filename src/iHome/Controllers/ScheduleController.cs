using iHome.Core.Services;
using iHome.Models.Requests.Schedules;
using iHome.Models.Responses.Schedules;
using iHome.Shared.Controllers;
using iHome.Shared.Logic;
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

    [HttpGet("GetScheduleDevices/{id}")]
    public async Task<IActionResult> GetScheduleDevices(Guid id)
    {
        var devices = await _scheduleService.GetScheduleDevices(id, _userAccessor.UserId);
        return Ok(devices);
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
