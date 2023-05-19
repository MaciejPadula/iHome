using iHome.Core.Models;
using iHome.Infrastructure.SQL.Models.Enums;

namespace iHome.Core.Repositories.Widgets;

public interface IWidgetRepository
{
    Task Add(WidgetType type, Guid roomId, bool showBorder, string userId);
    Task<List<WidgetModel>> GetByRoomId(Guid roomId);
    Task Remove(Guid widgetId);

}
