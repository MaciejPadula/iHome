using Dapper;

namespace iHome.Microservices.Widgets.Infrastructure.Repositories;

public class DapperWidgetDeviceRepository : IWidgetDeviceRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DapperWidgetDeviceRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task Add(Guid widgetId, Guid deviceId)
    {
        using var conn = _connectionFactory.GetConnection();

        await conn.ExecuteAsync(@"
INSERT INTO [maciejadmin].[WidgetsDevices]
    (Id, WidgetId, DeviceId)
VALUES
    (@Id, @WidgetId, @DeviceId)
", new { Id = Guid.NewGuid(), WidgetId = widgetId, DeviceId = deviceId });
    }

    public async Task<IEnumerable<Guid>> GetDeviceIdsByWidgetId(Guid widgetId)
    {
        using var conn = _connectionFactory.GetConnection();

        return await conn.QueryAsync<Guid>(@$"
SELECT DeviceId
FROM [maciejadmin].[WidgetsDevices]
WHERE WidgetId = @WidgetId
", new { WidgetId = widgetId });
    }

    public async Task Remove(Guid widgetId, Guid deviceId)
    {
        using var conn = _connectionFactory.GetConnection();

        await conn.ExecuteAsync(@"
DELETE FROM [maciejadmin].[WidgetsDevices]
WHERE WidgetId = @WidgetId AND DeviceId = @DeviceId
", new { WidgetId = widgetId, DeviceId = deviceId });
    }
}
