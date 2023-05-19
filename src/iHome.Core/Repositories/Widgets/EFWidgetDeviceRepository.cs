using iHome.Core.Exceptions.SqlExceptions;
using iHome.Core.Models;
using iHome.Infrastructure.SQL.Contexts;
using iHome.Infrastructure.SQL.Models.ConnectionTables;
using iHome.Infrastructure.SQL.Models.RootTables;
using Microsoft.EntityFrameworkCore;

namespace iHome.Core.Repositories.Widgets;

public class EFWidgetDeviceRepository : IWidgetDeviceRepository
{
    private readonly SqlDataContext _sqlDataContext;

    public EFWidgetDeviceRepository(SqlDataContext sqlDataContext)
    {
        _sqlDataContext = sqlDataContext;
    }

    public async Task Add(Guid widgetId, Guid deviceId)
    {
        var widget = await _sqlDataContext.Widgets
            .FirstOrDefaultAsync(w => w.Id == widgetId) ?? throw new WidgetNotFoundException();
        var device = await _sqlDataContext.Devices
            .FirstOrDefaultAsync(device => 
                device.RoomId == widget.RoomId && 
                device.Id == deviceId
            ) ?? throw new DeviceNotFoundException();

        if (await LimitReached(widget)) throw new MaxDevicesNumberReachedException();

        await _sqlDataContext.WidgetsDevices.AddAsync(new WidgetDevice
        {
            WidgetId = widgetId,
            DeviceId = deviceId
        });
        await _sqlDataContext.SaveChangesAsync();
    }

    public async Task<List<DeviceModel>> GetByWidgetId(Guid widgetId)
    {
        var widget = await _sqlDataContext.Widgets
            .Where(w => w.Id == widgetId)
            .Include(w => w.WidgetDevices)
            .ThenInclude(w => w.Device)
            .FirstOrDefaultAsync();

        return widget?.WidgetDevices
            .Select(w => w.Device)
            .OfType<Device>()
            .Select(d => new DeviceModel(d))
            .ToList() ?? Enumerable.Empty<DeviceModel>().ToList();
    }

    public async Task Remove(Guid widgetId, Guid deviceId)
    {
        var widgetDevice = await _sqlDataContext.WidgetsDevices
            .FirstOrDefaultAsync(x => x.DeviceId == deviceId && x.WidgetId == widgetId) ?? throw new DeviceNotFoundException();

        _sqlDataContext.WidgetsDevices.Remove(widgetDevice);
        await _sqlDataContext.SaveChangesAsync();
    }

    private async Task<bool> LimitReached(Widget widget)
    {
        var numberOfDevices = await _sqlDataContext.WidgetsDevices
            .CountAsync(widgetDevice => widgetDevice.WidgetId == widget.Id);

        return numberOfDevices >= widget.MaxNumberOfDevices;
    }
}
