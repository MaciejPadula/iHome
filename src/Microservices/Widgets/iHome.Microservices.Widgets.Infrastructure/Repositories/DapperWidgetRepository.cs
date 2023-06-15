using Dapper;
using iHome.Core.Repositories.Widgets;
using iHome.Infrastructure.Sql.Factories;
using iHome.Microservices.Widgets.Contract.Models;

namespace iHome.Microservices.Widgets.Infrastructure.Repositories;

public class DapperWidgetRepository : IWidgetRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DapperWidgetRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task Add(WidgetType type, Guid roomId, bool showBorder)
    {
        using var conn = _connectionFactory.GetOpenConnection();

        await conn.ExecuteAsync(@"
INSERT INTO [maciejadmin].[Widgets]
    (Id, WidgetType, RoomId, ShowBorder)
VALUES
    (@Id, @Type, @RoomId, @ShowBorder)
", new { Id = Guid.NewGuid(), Type = type, RoomId = roomId, ShowBorder = showBorder });
    }

    public async Task<IEnumerable<WidgetModel>> GetByRoomId(Guid roomId)
    {
        using var conn = _connectionFactory.GetOpenConnection();

        return await conn.QueryAsync<WidgetModel>(@$"
SELECT 
    Id AS {nameof(WidgetModel.Id)},
    WidgetType AS {nameof(WidgetModel.WidgetType)},
    RoomId AS {nameof(WidgetModel.RoomId)},
    ShowBorder AS {nameof(WidgetModel.ShowBorder)}
FROM [maciejadmin].[Widgets]
WHERE RoomId = @RoomId
", new { RoomId = roomId });
    }

    public async Task Remove(Guid widgetId)
    {
        using var conn = _connectionFactory.GetOpenConnection();

        await conn.ExecuteAsync(@"
DELETE FROM [maciejadmin].[Widgets]
WHERE Id = @Id
", new { Id = widgetId });
    }
}
