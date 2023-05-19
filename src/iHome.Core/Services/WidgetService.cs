using iHome.Core.Exceptions.SqlExceptions;
using iHome.Core.Logic.AccessGuards;
using iHome.Core.Models;
using iHome.Core.Repositories.Widgets;
using iHome.Infrastructure.SQL.Models.Enums;

namespace iHome.Core.Services;

public interface IWidgetService
{
    Task AddWidget(WidgetType type, Guid roomId, bool showBorder, string userId);
    Task InsertDevice(Guid widgetId, Guid deviceId, string userId);
    Task RemoveDevice(Guid widgetId, Guid deviceId, string userId);
    Task<IEnumerable<DeviceModel>> GetWidgetDevices(Guid widgetId, string userId);
    Task<IEnumerable<WidgetModel>> GetWidgets(Guid roomId, string userId);
    Task RemoveWidget(Guid widgetId, string userId);
}


public class WidgetService : IWidgetService
{
    private readonly IRoomAccessGuard _roomAccessGuard;
    private readonly IWidgetRepository _widgetRepository;
    private readonly IWidgetDeviceRepository _widgetDeviceRepository;

    public WidgetService(IRoomAccessGuard roomAccessGuard, IWidgetRepository widgetRepository, IWidgetDeviceRepository widgetDeviceRepository)
    {
        _roomAccessGuard = roomAccessGuard;
        _widgetRepository = widgetRepository;
        _widgetDeviceRepository = widgetDeviceRepository;
    }

    public async Task AddWidget(WidgetType type, Guid roomId, bool showBorder, string userId)
    {
        if (!await _roomAccessGuard.UserHasWriteAccess(roomId, userId)) throw new RoomNotFoundException();

        await _widgetRepository.Add(type, roomId, showBorder, userId);
    }

    public async Task<IEnumerable<DeviceModel>> GetWidgetDevices(Guid widgetId, string userId)
    {
        //validate

        return await _widgetDeviceRepository.GetByWidgetId(widgetId);
    }

    public async Task<IEnumerable<WidgetModel>> GetWidgets(Guid roomId, string userId)
    {
        if (!await _roomAccessGuard.UserHasWriteAccess(roomId, userId)) throw new RoomNotFoundException();

        return await _widgetRepository.GetByRoomId(roomId);
    }

    public async Task InsertDevice(Guid widgetId, Guid deviceId, string userId)
    {
        //validate
        await _widgetDeviceRepository.Add(widgetId, deviceId);
    }

    public async Task RemoveDevice(Guid widgetId, Guid deviceId, string userId)
    {
        // validate

        await _widgetDeviceRepository.Remove(widgetId, deviceId);
    }

    //private async Task<Widget> GetWidget(Guid widgetId, string userId)
    //{
    //    var widget = await _sqlDataContext.Widgets.FirstOrDefaultAsync(widget => widget.Id == widgetId) ?? throw new Exception();

    //    if (!await _roomAccessGuard.UserHasWriteAccess(widget.RoomId, userId)) throw new RoomNotFoundException();

    //    return widget;
    //}

    public async Task RemoveWidget(Guid widgetId, string userId)
    {
        //validate

        await _widgetRepository.Remove(widgetId);
    }
}
