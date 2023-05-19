using iHome.Core.Models;

namespace iHome.Core.Repositories.Widgets;

public interface IWidgetDeviceRepository
{
    Task Add(Guid widgetId, Guid deviceId);
    Task<List<DeviceModel>> GetByWidgetId(Guid widgetId);
    Task Remove(Guid widgetId, Guid deviceId);
}
