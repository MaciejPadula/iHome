using iHome.Core.Services;
using iHome.Models.Requests.Widgets;
using iHome.Models.Responses.Widget;
using iHome.Shared.Controllers;
using iHome.Shared.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class WidgetController : BaseApiController
{
    private readonly IWidgetService _widgetService;

    public WidgetController(IWidgetService widgetService, IUserAccessor userAccessor)
        : base(userAccessor)
    {
        _widgetService = widgetService;
    }

    [HttpPost("AddWidget")]
    public async Task<IActionResult> AddWidget([FromBody] AddWidgetRequest request)
    {
        var widgetId = await _widgetService.AddWidget(
            request.RoomId, request.WidgetType, request.ShowBorder, _userAccessor.UserId);
        return Ok(new AddWidgetResponse
        {
            WidgetId = widgetId
        });
    }

    [HttpPost("InsertDevice")]
    public async Task<IActionResult> InsertDevice([FromBody] InsertDeviceRequest request)
    {
        await _widgetService.InsertDevice(
            request.DeviceId, request.WidgetId, _userAccessor.UserId);
        return Ok();
    }

    [HttpPost("RemoveDevice")]
    public async Task<IActionResult> RemoveDevice([FromBody] RemoveWidgetDeviceRequest request)
    {
        await _widgetService.RemoveDevice(
            request.DeviceId, request.WidgetId, _userAccessor.UserId);
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
        await _widgetService.Remove(widgetId, _userAccessor.UserId);
        return Ok();
    }
}
