using iHome.Core.Services.Widgets;
using iHome.Logic;
using iHome.Models.Requests;
using iHome.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Controllers;

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
    public IActionResult AddWidget([FromBody] AddWidgetRequest request)
    {
        _widgetService.AddWidget(request.WidgetType, request.RoomId, request.ShowBorder, _userAccessor.UserId);
        return Ok();
    }

    [HttpPost("InsertDevice")]
    public IActionResult InsertDevice([FromBody] InsertDeviceRequest request)
    {
        _widgetService.InsertDevice(request.WidgetId, request.DeviceId, _userAccessor.UserId);
        return Ok();
    }

    [HttpPost("RemoveDevice")]
    public IActionResult RemoveDevice([FromBody] RemoveWidgetDeviceRequest request)
    {
        _widgetService.RemoveDevice(request.WidgetId, request.DeviceId, _userAccessor.UserId);
        return Ok();
    }

    [HttpGet("GetWidgets/{roomId}")]
    public IActionResult GetWidgets(Guid roomId)
    {
        return Ok(_widgetService.GetWidgets(roomId, _userAccessor.UserId)
            .Select(w => new GetWidgetsWidget
            {
                Id = w.Id,
                RoomId = roomId,
                MaxNumberOfDevices = w.MaxNumberOfDevices,
                WidgetType = w.WidgetType,
                ShowBorder = w.ShowBorder
            }));
    }

    [HttpGet("GetWidgetDevices/{widgetId}")]
    public IActionResult GetWidgetDevices(Guid widgetId)
    {
        return Ok(_widgetService.GetWidgetDevices(widgetId, _userAccessor.UserId));
    }

    [HttpDelete("RemoveWidget/{widgetId}")]
    public IActionResult RemoveWidget(Guid widgetId)
    {
        _widgetService.RemoveWidget(widgetId, _userAccessor.UserId);
        return Ok();
    }
}
