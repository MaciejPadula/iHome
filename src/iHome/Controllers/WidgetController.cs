using iHome.Core.Models;
using iHome.Core.Services;
using iHome.Logic;
using iHome.Microservices.Widgets.Contract;
using iHome.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class WidgetController : ControllerBase
{
    private readonly IWidgetManagementService _widgetManagementService;
    private readonly IWidgetDeviceManagementService _widgetDeviceManagementService;
    private readonly IDeviceService _deviceService;

    private readonly IUserAccessor _userAccessor;

    public WidgetController(IWidgetManagementService widgetManagementService, IWidgetDeviceManagementService widgetDeviceManagementService, IUserAccessor userAccessor, IDeviceService deviceService)
    {
        _widgetManagementService = widgetManagementService;
        _widgetDeviceManagementService = widgetDeviceManagementService;
        _userAccessor = userAccessor;
        _deviceService = deviceService;
    }

    [HttpPost("AddWidget")]
    public async Task<IActionResult> AddWidget([FromBody] AddWidgetRequest request)
    {
        await _widgetManagementService.AddWidget(new()
        {
            Type = request.WidgetType,
            RoomId = request.RoomId,
            ShowBorder = request.ShowBorder,
            UserId = _userAccessor.UserId
        });
        return Ok();
    }

    [HttpPost("InsertDevice")]
    public async Task<IActionResult> InsertDevice([FromBody] InsertDeviceRequest request)
    {
        await _widgetDeviceManagementService.InsertDevice(new()
        {
            DeviceId = request.DeviceId,
            WidgetId = request.WidgetId,
            UserId = _userAccessor.UserId
        });
        return Ok();
    }

    [HttpPost("RemoveDevice")]
    public async Task<IActionResult> RemoveDevice([FromBody] RemoveWidgetDeviceRequest request)
    {
        await _widgetDeviceManagementService.RemoveDevice(new()
        {
            DeviceId = request.DeviceId,
            WidgetId = request.WidgetId,
            UserId = _userAccessor.UserId
        });
        return Ok();
    }

    [HttpGet("GetWidgets/{roomId}")]
    public async Task<IActionResult> GetWidgets(Guid roomId)
    {
        var response = await _widgetManagementService.GetWidgets(new()
        {
            RoomId = roomId,
            UserId = _userAccessor.UserId
        });

        return Ok(response.Widgets);
    }

    [HttpGet("GetWidgetDevices/{widgetId}")]
    public async Task<IActionResult> GetWidgetDevices(Guid widgetId)
    {
        var response = await _widgetDeviceManagementService.GetWidgetDevicesIds(new()
        {
            WidgetId = widgetId,
            UserId = _userAccessor.UserId
        });

        var deviceIds = response.DevicesIds.ToList();
        var devices = new List<DeviceModel>();

        foreach (var deviceId in  deviceIds)
        {
            var device = await _deviceService.GetDevice(deviceId, _userAccessor.UserId);
            if (device == null) continue;

            devices.Add(device);
        }


        return Ok(devices);
    }

    [HttpDelete("RemoveWidget/{widgetId}")]
    public async Task<IActionResult> RemoveWidget(Guid widgetId)
    {
        await _widgetManagementService.RemoveWidget(new()
        {
            WidgetId = widgetId,
            UserId = _userAccessor.UserId
        });
        return Ok();
    }
}
