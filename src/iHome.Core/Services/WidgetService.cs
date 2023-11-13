using iHome.Core.Helpers;
using iHome.Core.Models;
using iHome.Core.Services.Validation;
using iHome.Microservices.Devices.Contract;
using iHome.Microservices.Devices.Contract.Models;
using iHome.Microservices.Widgets.Contract;
using iHome.Microservices.Widgets.Contract.Models;
using iHome.Microservices.Widgets.Contract.Models.Request;

namespace iHome.Core.Services;

public interface IWidgetService
{
    Task<Guid> AddWidget(Guid roomId, WidgetType type, bool showBorders, string userId);
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

    private readonly IValidationService _validationService;

    public WidgetService(IWidgetDeviceManagementService widgetDeviceManagementService,
        IDeviceManagementService deviceManagementService,
        IWidgetManagementService widgetManagementService,
        IValidationService validationService)
    {
        _widgetDeviceManagementService = widgetDeviceManagementService;
        _deviceManagementService = deviceManagementService;
        _widgetManagementService = widgetManagementService;
        _validationService = validationService;
    }

    public async Task<Guid> AddWidget(Guid roomId, WidgetType type, bool showBorders, string userId)
    {
        var request = new AddWidgetRequest
        {
            Type = type,
            RoomId = roomId,
            ShowBorder = showBorders,
            UserId = userId
        };

        var response = await _validationService.Validate(roomId, userId, ValidatorType.RoomWrite, () => _widgetManagementService.AddWidget(request));
        return response.WidgetId;
    }

    public async Task<List<DeviceModel>> GetWidgetDevices(Guid widgetId, string userId)
    {
        var request = new GetWidgetDevicesIdsRequest
        {
            WidgetId = widgetId,
            UserId = userId
        };

        var response = await _validationService.Validate(widgetId, userId, ValidatorType.WidgetRead, () => _widgetDeviceManagementService.GetWidgetDevicesIds(request));

        var devices = await _deviceManagementService.GetDevicesByIds(new()
        {
            DeviceIds = response.DevicesIds.ToList(),
            UserId = userId
        });

        return devices?.Devices?.ToList() ?? ListUtils.Empty<DeviceModel>();
    }

    public async Task<List<WidgetModel>> GetWidgets(Guid roomId, string userId)
    {
        var request = new GetWidgetsRequest
        {
            RoomId = roomId,
            UserId = userId
        };

        var widgets = await _validationService.Validate(roomId, userId, ValidatorType.RoomRead, () => _widgetManagementService.GetWidgets(request));

        return widgets?.Widgets?.ToList() ?? Enumerable.Empty<WidgetModel>().ToList();
    }

    public async Task InsertDevice(Guid deviceId, Guid widgetId, string userId)
    {
        var request = new InsertDeviceRequest
        {
            DeviceId = deviceId,
            WidgetId = widgetId,
            UserId = userId
        };

        await _validationService.Validate(widgetId, userId, ValidatorType.WidgetWrite, () => _widgetDeviceManagementService.InsertDevice(request));
    }

    public async Task Remove(Guid widgetId, string userId)
    {
        var request = new RemoveWidgetRequest
        {
            WidgetId = widgetId,
            UserId = userId
        };

        await _validationService.Validate(widgetId, userId, ValidatorType.WidgetWrite, () => _widgetManagementService.RemoveWidget(request));
    }

    public async Task RemoveDevice(Guid deviceId, Guid widgetId, string userId)
    {
        var request = new RemoveDeviceRequest
        {
            DeviceId = deviceId,
            WidgetId = widgetId,
            UserId = userId
        };

        await _validationService.Validate(widgetId, userId, ValidatorType.WidgetWrite, () => _widgetDeviceManagementService.RemoveDevice(request));
    }
}
