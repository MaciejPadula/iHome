using iHome.Microservices.Devices.Contract;
using iHome.Microservices.Devices.Contract.Models;
using iHome.Microservices.Widgets.Contract;
using iHome.Microservices.Widgets.Contract.Models;

namespace iHome.Core.Services;

public interface IWidgetService
{
    Task AddWidget(Guid roomId, WidgetType type, bool showBorders, string userId);
    Task InsertDevice(Guid deviceId, Guid widgetId, string userId);
    Task RemoveDevice(Guid deviceId, Guid widgetId, string userId);
    Task<List<WidgetModel>> GetWidgets(Guid roomId, string userId);
    Task<List<DeviceModel>> GetWidgetDevices(Guid widgetId, string userId);
    Task Remove(Guid widgetId, string userId);
}

public class WidgetService : IWidgetService
{
    private readonly IWidgetManagementService _widgetManagementService;
    private readonly IWidgetDeviceManagementService _widgetDeviceManagementService;
    private readonly IDeviceManagementService _deviceManagementService;

    public WidgetService(IWidgetDeviceManagementService widgetDeviceManagementService, IDeviceManagementService deviceManagementService, IWidgetManagementService widgetManagementService)
    {
        _widgetDeviceManagementService = widgetDeviceManagementService;
        _deviceManagementService = deviceManagementService;
        _widgetManagementService = widgetManagementService;
    }

    public async Task AddWidget(Guid roomId, WidgetType type, bool showBorders, string userId)
    {
        await _widgetManagementService.AddWidget(new()
        {
            Type = type,
            RoomId = roomId,
            ShowBorder = showBorders,
            UserId = userId
        });
    }

    public async Task<List<DeviceModel>> GetWidgetDevices(Guid widgetId, string userId)
    {
        var response = await _widgetDeviceManagementService.GetWidgetDevicesIds(new()
        {
            WidgetId = widgetId,
            UserId = userId
        });

        var deviceIds = response.DevicesIds.ToList();
        var devices = new List<DeviceModel>();

        foreach (var deviceId in deviceIds)
        {
            var device = await _deviceManagementService.GetDevice(new()
            {
                DeviceId = deviceId
            });
            if (device?.Device == null) continue;

            devices.Add(device.Device);
        }

        return devices;
    }

    public async Task<List<WidgetModel>> GetWidgets(Guid roomId, string userId)
    {
        var widgets = await _widgetManagementService.GetWidgets(new()
        {
            RoomId = roomId,
            UserId = userId
        });

        return widgets?.Widgets?.ToList() ?? Enumerable.Empty<WidgetModel>().ToList();
    }

    public async Task InsertDevice(Guid deviceId, Guid widgetId, string userId)
    {
        await _widgetDeviceManagementService.InsertDevice(new()
        {
            DeviceId = deviceId,
            WidgetId = widgetId,
            UserId = userId
        });
    }

    public async Task Remove(Guid widgetId, string userId)
    {
        await _widgetManagementService.RemoveWidget(new()
        {
            WidgetId = widgetId,
            UserId = userId
        });
    }

    public async Task RemoveDevice(Guid deviceId, Guid widgetId, string userId)
    {
        await _widgetDeviceManagementService.RemoveDevice(new()
        {
            DeviceId = deviceId,
            WidgetId = widgetId,
            UserId = userId
        });
    }
}
