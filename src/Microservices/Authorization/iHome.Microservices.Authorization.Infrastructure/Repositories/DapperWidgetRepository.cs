using Dapper;
using iHome.Infrastructure.Sql.Factories;
using iHome.Infrastructure.Sql.Repositories;
using iHome.Microservices.Authorization.Domain.Repositories;

namespace iHome.Microservices.Authorization.Infrastructure.Repositories;

public class DapperWidgetRepository : RepositoryBase, IWidgetRepository
{
    public DapperWidgetRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public async Task<bool> UserHasReadAccess(Guid widgetId, string userId)
    {
        using var conn = GetDbConnection();

        return await conn.ExecuteScalarAsync<bool>(@"
SELECT 1
FROM ([maciejadmin].[Rooms] r
LEFT JOIN [maciejadmin].[UsersRooms] ur
    ON r.Id = ur.RoomId)
INNER JOIN [maciejadmin].[Widgets] w ON r.Id = w.RoomId
WHERE w.Id = @WidgetId AND (r.UserId = @UserId OR ur.UserId = @UserId)
", new { WidgetId = widgetId, UserId = userId });
    }

    public async Task<bool> UserHasWriteAccess(Guid widgetId, string userId)
    {
        using var conn = GetDbConnection();

        return await conn.ExecuteScalarAsync<bool>(@"
SELECT 1
FROM [maciejadmin].[Rooms] r
INNER JOIN [maciejadmin].[Widgets] w ON r.Id = w.RoomId
WHERE w.Id = @WidgetId AND r.UserId = @UserId
", new { WidgetId = widgetId, UserId = userId });
    }
}
