using iHome.DevicesScheduling.Api.Request;
using iHome.DevicesScheduling.Api.Response;
using iHome.DevicesScheduling.Features.GetDevicesForScheduling;
using iHome.DevicesScheduling.Features.GetScheduleDevices;
using iHome.DevicesScheduling.Features.ScheduleOrUpdateDevice;
using iHome.Shared.Controllers;
using iHome.Shared.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Infrastructure.Cqrs.Mediator;

namespace iHome.DevicesScheduling.Api;

[Authorize]
public class DevicesSchedulingController : BaseApiController
{
    private readonly IMediator _mediator;

    public DevicesSchedulingController(IUserAccessor userAccessor, IMediator mediator) : base(userAccessor)
    {
        _mediator = mediator;
    }

    [HttpPost("GetDevicesForScheduling")]
    public async Task<IActionResult> GetDevicesForScheduling()
    {
        var query = new GetDevicesForSchedulingQuery { UserId = _userAccessor.UserId };
        await _mediator.HandleQueryAsync(query);

        return Ok(new GetDevicesForSchedulingResponse
        {
            Devices = query.Result
        });
    }

    [HttpPost("AddOrUpdateScheduleDevice")]
    public async Task<IActionResult> AddOrUpdateScheduleDevice([FromBody] AddOrUpdateScheduleDeviceRequest request)
    {
        var query = new ScheduleOrUpdateDeviceQuery
        {
            ScheduleId = request.ScheduleId,
            DeviceId = request.DeviceId,
            DeviceData = request.DeviceData,
            UserId = _userAccessor.UserId
        };
        await _mediator.HandleQueryAsync(query);

        return Ok(new AddOrUpdateScheduleDeviceResponse
        {
            ScheduleDeviceId = query.Result
        });
    }

    [HttpGet("GetScheduleDevices/{id}")]
    public async Task<IActionResult> GetScheduleDevices(Guid id)
    {
        var query = new GetScheduleDevicesQuery
        {
            ScheduleId = id,
            UserId = _userAccessor.UserId
        };
        await _mediator.HandleQueryAsync(query);

        return Ok(new GetScheduleDevicesResponse
        {
            ScheduleDevices = query.Result
        });
    }
}
