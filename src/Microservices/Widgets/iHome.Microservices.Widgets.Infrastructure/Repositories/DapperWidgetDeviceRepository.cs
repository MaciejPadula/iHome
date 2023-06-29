using Dapper;
using iHome.Infrastructure.Sql.Factories;
using iHome.Infrastructure.Sql.Repositories;

namespace iHome.Microservices.Widgets.Infrastructure.Repositories;

public class DapperWidgetDeviceRepository : RepositoryBase, IWidgetDeviceRepository
{
    public DapperWidgetDeviceRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public async Task Add(Guid widgetId, Guid deviceId)
    {
        using var conn = GetDbConnection();

        await conn.ExecuteAsync(@"
INSERT INTO [maciejadmin].[WidgetsDevices]
    (Id, WidgetId, DeviceId)
VALUES
    (@Id, @WidgetId, @DeviceId)
", new { Id = Guid.NewGuid(), WidgetId = widgetId, DeviceId = deviceId });
    }

    public async Task<IEnumerable<Guid>> GetDeviceIdsByWidgetId(Guid widgetId)
    {
        using var conn = GetDbConnection();

        return await conn.QueryAsync<Guid>(@$"
SELECT DeviceId
FROM [maciejadmin].[WidgetsDevices]
WHERE WidgetId = @WidgetId
", new { WidgetId = widgetId });
    }

    public async Task Remove(Guid widgetId, Guid deviceId)
    {
        using var conn = GetDbConnection();

        await conn.ExecuteAsync(@"
DELETE FROM [maciejadmin].[WidgetsDevices]
WHERE WidgetId = @WidgetId AND DeviceId = @DeviceId
", new { WidgetId = widgetId, DeviceId = deviceId });
    }
}
