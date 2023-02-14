using iHome.Core.Exceptions;
using iHome.Core.Models;
using iHome.Core.Repositories;
using iHome.Core.Services.Rooms;
using iHome.Devices.Contract.Models;

namespace iHome.Core.Services.Widgets;

public class WidgetService : IWidgetService
{
    private readonly IRoomService _roomService;
    private readonly SqlDataContext _sqlDataContext;

    public WidgetService(IRoomService roomService, SqlDataContext sqlDataContext)
    {
        _roomService = roomService;
        _sqlDataContext = sqlDataContext;
    }

    public void AddWidget(WidgetType type, Guid roomId, bool showBorder, string userId)
    {
        if (!_roomService.UserCanAccessRoom(roomId, userId)) throw new RoomNotFoundException();

        _sqlDataContext.Widgets.Add(new Widget
        {
            WidgetType = type,
            RoomId = roomId,
            ShowBorder = showBorder,
        });
        _sqlDataContext.SaveChanges();
    }

    public IEnumerable<Device> GetWidgetDevices(Guid widgetId, string userId)
    {
        var widget = _sqlDataContext.Widgets.FirstOrDefault(widget => widget.Id == widgetId);
        if (widget == null) throw new Exception();

        if (!_roomService.UserCanAccessRoom(widget.RoomId, userId)) throw new RoomNotFoundException();

        return _sqlDataContext.GetWidgetDevices(widgetId);
    }

    public IEnumerable<Widget> GetWidgets(Guid roomId, string userId)
    {
        if (!_roomService.UserCanAccessRoom(roomId, userId)) throw new RoomNotFoundException();

        return _sqlDataContext.GetRoomWidgets(roomId).ToList();
    }

    public void InsertDevice(Guid widgetId, Guid deviceId, string userId)
    {
        var widget = GetWidget(widgetId, userId);

        var device = _sqlDataContext.Devices.FirstOrDefault(device => device.Id == deviceId);
        if(device == null || device.RoomId != widget.RoomId) throw new DeviceNotFoundException();

        if(_sqlDataContext.WidgetsDevices
                .Any(widgetDevice => widgetDevice.WidgetId == widgetId && widgetDevice.DeviceId == deviceId) ||
           _sqlDataContext.WidgetsDevices.Count(widgetDevice => widgetDevice.WidgetId == widgetId) >= widget.MaxNumberOfDevices) throw new Exception();

        _sqlDataContext.WidgetsDevices.Add(new WidgetDevice
        {
            WidgetId = widgetId,
            DeviceId = deviceId
        });
        _sqlDataContext.SaveChanges();
    }

    public void RemoveDevice(Guid widgetId, Guid deviceId, string userId)
    {
        var widget = GetWidget(widgetId, userId);

        var device = _sqlDataContext.Devices.FirstOrDefault(device => device.Id == deviceId);
        if (device == null || device.RoomId != widget.RoomId) throw new DeviceNotFoundException();

        var widgetDevice = _sqlDataContext.WidgetsDevices.FirstOrDefault(x => x.DeviceId == deviceId && x.WidgetId == widget.Id);
        if(widgetDevice == null) throw new DeviceNotFoundException();

        _sqlDataContext.WidgetsDevices.Remove(widgetDevice);
        _sqlDataContext.SaveChanges();
    }

    private Widget GetWidget(Guid widgetId, string userId)
    {
        var widget = _sqlDataContext.Widgets.FirstOrDefault(widget => widget.Id == widgetId);
        if (widget == null) throw new Exception();

        if (!_roomService.UserCanAccessRoom(widget.RoomId, userId)) throw new RoomNotFoundException();

        return widget;
    }

    public void RemoveWidget(Guid widgetId, string userId)
    {
        var widget = _sqlDataContext.Widgets.FirstOrDefault(widget => widget.Id == widgetId);
        if (widget == null) throw new Exception();

        if (!_roomService.UserCanAccessRoom(widget.RoomId, userId)) throw new RoomNotFoundException();
        
        _sqlDataContext.Widgets.Remove(widget);
        _sqlDataContext.SaveChanges();
    }
}
