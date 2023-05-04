using iHome.Core.Models;
using iHome.Infrastructure.SQL.Models.Enums;

namespace iHome.Core.Services.Widgets;

public interface IWidgetService
{
    Task AddWidget(WidgetType type, Guid roomId, bool showBorder, string userId);
    Task InsertDevice(Guid widgetId, Guid deviceId, string userId);
    Task RemoveDevice(Guid widgetId, Guid deviceId, string userId);
    Task<IEnumerable<DeviceModel>> GetWidgetDevices(Guid widgetId, string userId);
    Task<IEnumerable<WidgetModel>> GetWidgets(Guid roomId, string userId);
    Task RemoveWidget(Guid widgetId, string userId);
}
