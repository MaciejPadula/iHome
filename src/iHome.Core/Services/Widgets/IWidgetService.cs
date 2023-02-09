using iHome.Core.Models;

namespace iHome.Core.Services.Widgets;

public interface IWidgetService
{
    void AddWidget(WidgetType type, Guid roomId, string userId);
    void InsertDevice(Guid widgetId, Guid deviceId, string userId);
    IEnumerable<Widget> GetWidgets(Guid roomId, string userId);
    void RemoveWidget(Guid widgetId, string userId);
}
