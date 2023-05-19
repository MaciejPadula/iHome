using iHome.Core.Exceptions.SqlExceptions;
using iHome.Core.Models;
using iHome.Infrastructure.SQL.Contexts;
using iHome.Infrastructure.SQL.Models.Enums;
using iHome.Infrastructure.SQL.Models.RootTables;
using Microsoft.EntityFrameworkCore;

namespace iHome.Core.Repositories.Widgets;

public class EFWidgetRepository : IWidgetRepository
{
    private readonly SqlDataContext _sqlDataContext;

    public EFWidgetRepository(SqlDataContext sqlDataContext)
    {
        _sqlDataContext = sqlDataContext;
    }

    public async Task Add(WidgetType type, Guid roomId, bool showBorder, string userId)
    {
        await _sqlDataContext.Widgets.AddAsync(new Widget
        {
            WidgetType = type,
            RoomId = roomId,
            ShowBorder = showBorder,
        });
        await _sqlDataContext.SaveChangesAsync();
    }

    public async Task<List<WidgetModel>> GetByRoomId(Guid roomId)
    {
        var roomWithWidgets = await _sqlDataContext.Rooms
            .Where(r => r.Id == roomId)
            .Include(context => context.Widgets)
            .FirstOrDefaultAsync();

        return roomWithWidgets?.Widgets
            .Select(w => new WidgetModel(w))
            .ToList() ?? Enumerable.Empty<WidgetModel>().ToList();
    }

    public async Task Remove(Guid widgetId)
    {
        var widget = await _sqlDataContext.Widgets
            .FirstOrDefaultAsync(widget => widget.Id == widgetId) ?? throw new WidgetNotFoundException();

        _sqlDataContext.Widgets.Remove(widget);
        await _sqlDataContext.SaveChangesAsync();
    }
}
