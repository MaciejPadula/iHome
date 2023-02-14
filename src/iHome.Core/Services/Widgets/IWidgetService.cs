using iHome.Core.Models;
using iHome.Devices.Contract.Models;

namespace iHome.Core.Services.Widgets;

public interface IWidgetService
{
    void AddWidget(WidgetType type, Guid roomId, bool showBorder, string userId);
    void InsertDevice(Guid widgetId, Guid deviceId, string userId);
    void RemoveDevice(Guid widgetId, Guid deviceId, string userId);
    IEnumerable<Device> GetWidgetDevices(Guid widgetId, string userId);
    IEnumerable<Widget> GetWidgets(Guid roomId, string userId);
    void RemoveWidget(Guid widgetId, string userId);
}
