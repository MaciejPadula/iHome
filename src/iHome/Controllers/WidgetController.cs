using iHome.Core.Services.Widgets;
using iHome.Logic;
using iHome.Models;
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
        _widgetService.AddWidget(request.WidgetType, request.RoomId, _userAccessor.UserId);
        return Ok();
    }

    [HttpGet("GetWidgets/{id}")]
    public IActionResult GetWidgets(Guid roomId)
    {
        return Ok(_widgetService.GetWidgets(roomId, _userAccessor.UserId));
    }

    [HttpDelete("RemoveWidget/{id}")]
    public IActionResult RemoveWidget(Guid id)
    {
        _widgetService.RemoveWidget(id, _userAccessor.UserId);
        return Ok();
    }
}
