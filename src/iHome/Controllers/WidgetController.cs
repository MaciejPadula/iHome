using iHome.Core.Services;
using iHome.Logic;
using iHome.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class WidgetController : ControllerBase
{
    private readonly IUserAccessor _userAccessor;
    private readonly IWidgetService _widgetService;

    public WidgetController(IUserAccessor userAccessor, IWidgetService widgetService)
    {
        _userAccessor = userAccessor;
        _widgetService = widgetService;
    }

    [HttpPost("AddWidget")]
    public async Task<IActionResult> AddWidget([FromBody] AddWidgetRequest request)
    {
        await _widgetService.AddWidget(request.WidgetType, request.RoomId, request.ShowBorder, _userAccessor.UserId);
        return Ok();
    }

    [HttpPost("InsertDevice")]
    public async Task<IActionResult> InsertDevice([FromBody] InsertDeviceRequest request)
    {
        await _widgetService.InsertDevice(request.WidgetId, request.DeviceId, _userAccessor.UserId);
        return Ok();
    }

    [HttpPost("RemoveDevice")]
    public async Task<IActionResult> RemoveDevice([FromBody] RemoveWidgetDeviceRequest request)
    {
        await _widgetService.RemoveDevice(request.WidgetId, request.DeviceId, _userAccessor.UserId);
        return Ok();
    }

    [HttpGet("GetWidgets/{roomId}")]
    public async Task<IActionResult> GetWidgets(Guid roomId)
    {
        var widgets = await _widgetService.GetWidgets(roomId, _userAccessor.UserId);

        return Ok(widgets);
    }

    [HttpGet("GetWidgetDevices/{widgetId}")]
    public async Task<IActionResult> GetWidgetDevices(Guid widgetId)
    {
        var devices = await _widgetService.GetWidgetDevices(widgetId, _userAccessor.UserId);
        return Ok(devices);
    }

    [HttpDelete("RemoveWidget/{widgetId}")]
    public async Task<IActionResult> RemoveWidget(Guid widgetId)
    {
        await _widgetService.RemoveWidget(widgetId, _userAccessor.UserId);
        return Ok();
    }
}
