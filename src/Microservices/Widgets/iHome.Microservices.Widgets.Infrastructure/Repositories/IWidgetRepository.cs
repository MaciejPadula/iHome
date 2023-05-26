using iHome.Microservices.Widgets.Contract.Models;

namespace iHome.Core.Repositories.Widgets;

public interface IWidgetRepository
{
    Task Add(WidgetType type, Guid roomId, bool showBorder);
    Task<IEnumerable<WidgetModel>> GetByRoomId(Guid roomId);
    Task Remove(Guid widgetId);
}
