using iHome.Core.Exceptions;
using iHome.Core.Exceptions.SqlExceptions;
using iHome.Core.Services.Rooms;
using iHome.Infrastructure.SQL.Contexts;
using iHome.Infrastructure.SQL.Models;
using Microsoft.EntityFrameworkCore;

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

    public async Task AddWidget(WidgetType type, Guid roomId, bool showBorder, string userId)
    {
        if (!await _roomService.UserCanAccessRoom(roomId, userId)) throw new RoomNotFoundException();

        await _sqlDataContext.Widgets.AddAsync(new Widget
        {
            WidgetType = type,
            RoomId = roomId,
            ShowBorder = showBorder,
        });
        await _sqlDataContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Device>> GetWidgetDevices(Guid widgetId, string userId)
    {
        var widget = await _sqlDataContext.Widgets
            .Where(w => w.Id == widgetId)
            .Include(w => w.WidgetDevices)
            .ThenInclude(w => w.Device)
            .FirstOrDefaultAsync();

        if (widget == null) throw new Exception();

        if (!await _roomService.UserCanAccessRoom(widget.RoomId, userId)) throw new RoomNotFoundException();

        return widget.WidgetDevices.Select(w => w.Device).OfType<Device>() ?? Enumerable.Empty<Device>();
    }

    public async Task<IEnumerable<Widget>> GetWidgets(Guid roomId, string userId)
    {
        if (!await _roomService.UserCanAccessRoom(roomId, userId)) throw new RoomNotFoundException();

        var roomWithWidgets = await _sqlDataContext.Rooms
            .Where(r => r.Id == roomId)
            .Include(context => context.Widgets)
            .FirstOrDefaultAsync();

        if (roomWithWidgets == null) throw new Exception();

        return roomWithWidgets.Widgets ?? Enumerable.Empty<Widget>();
    }

    public async Task InsertDevice(Guid widgetId, Guid deviceId, string userId)
    {
        var widget = await GetWidget(widgetId, userId);

        var device = await _sqlDataContext.Devices.FirstOrDefaultAsync(device => device.Id == deviceId);
        if (device == null || device.RoomId != widget.RoomId) throw new DeviceNotFoundException();

        if (await _sqlDataContext.WidgetsDevices
                .AnyAsync(widgetDevice => widgetDevice.WidgetId == widgetId && widgetDevice.DeviceId == deviceId) ||
           await _sqlDataContext.WidgetsDevices.CountAsync(widgetDevice => widgetDevice.WidgetId == widgetId) >= widget.MaxNumberOfDevices) throw new Exception();

        await _sqlDataContext.WidgetsDevices.AddAsync(new WidgetDevice
        {
            WidgetId = widgetId,
            DeviceId = deviceId
        });
        await _sqlDataContext.SaveChangesAsync();
    }

    public async Task RemoveDevice(Guid widgetId, Guid deviceId, string userId)
    {
        var widget = await GetWidget(widgetId, userId);

        var device = await _sqlDataContext.Devices.FirstOrDefaultAsync(device => device.Id == deviceId);
        if (device == null || device.RoomId != widget.RoomId) throw new DeviceNotFoundException();

        var widgetDevice = await _sqlDataContext.WidgetsDevices.FirstOrDefaultAsync(x => x.DeviceId == deviceId && x.WidgetId == widget.Id);
        if (widgetDevice == null) throw new DeviceNotFoundException();

        _sqlDataContext.WidgetsDevices.Remove(widgetDevice);
        await _sqlDataContext.SaveChangesAsync();
    }

    private async Task<Widget> GetWidget(Guid widgetId, string userId)
    {
        var widget = await _sqlDataContext.Widgets.FirstOrDefaultAsync(widget => widget.Id == widgetId);
        if (widget == null) throw new Exception();

        if (!await _roomService.UserCanAccessRoom(widget.RoomId, userId)) throw new RoomNotFoundException();

        return widget;
    }

    public async Task RemoveWidget(Guid widgetId, string userId)
    {
        var widget = await _sqlDataContext.Widgets.FirstOrDefaultAsync(widget => widget.Id == widgetId);
        if (widget == null) throw new Exception();

        if (!await _roomService.UserCanAccessRoom(widget.RoomId, userId)) throw new RoomNotFoundException();

        _sqlDataContext.Widgets.Remove(widget);
        await _sqlDataContext.SaveChangesAsync();
    }
}
