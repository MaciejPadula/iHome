using iHome.Infrastructure.SQL.Models;

namespace iHome.Core.Services.Widgets;

public interface IWidgetService
{
    Task AddWidget(WidgetType type, Guid roomId, bool showBorder, string userId);
    Task InsertDevice(Guid widgetId, Guid deviceId, string userId);
    Task RemoveDevice(Guid widgetId, Guid deviceId, string userId);
    Task<IEnumerable<Device>> GetWidgetDevices(Guid widgetId, string userId);
    Task<IEnumerable<Widget>> GetWidgets(Guid roomId, string userId);
    Task RemoveWidget(Guid widgetId, string userId);
}
