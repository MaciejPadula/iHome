using Dapper;
using iHome.Core.Repositories.Widgets;
using iHome.Infrastructure.Sql.Factories;
using iHome.Infrastructure.Sql.Repositories;
using iHome.Microservices.Widgets.Contract.Models;

namespace iHome.Microservices.Widgets.Infrastructure.Repositories;

public class DapperWidgetRepository : RepositoryBase, IWidgetRepository
{
    public DapperWidgetRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public async Task Add(WidgetType type, Guid roomId, bool showBorder)
    {
        using var conn = GetDbConnection();

        await conn.ExecuteAsync(@"
INSERT INTO [maciejadmin].[Widgets]
    (Id, WidgetType, RoomId, ShowBorder)
VALUES
    (@Id, @Type, @RoomId, @ShowBorder)
", new { Id = Guid.NewGuid(), Type = type, RoomId = roomId, ShowBorder = showBorder });
    }

    public async Task<IEnumerable<WidgetModel>> GetByRoomId(Guid roomId)
    {
        using var conn =GetDbConnection();

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
        using var conn = GetDbConnection();

        await conn.ExecuteAsync(@"
DELETE FROM [maciejadmin].[Widgets]
WHERE Id = @Id
", new { Id = widgetId });
    }
}
