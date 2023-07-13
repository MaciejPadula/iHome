using Dapper;
using iHome.Infrastructure.Sql.Factories;
using iHome.Infrastructure.Sql.Repositories;

namespace iHome.Microservices.Authorization.Infrastructure.Repositories;

public class DapperDeviceRepository : RepositoryBase, IDeviceRepository
{
    public DapperDeviceRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public async Task<bool> UserHasReadAccess(Guid deviceId, string userId)
    {
        using var conn = GetDbConnection();

        return await conn.ExecuteScalarAsync<bool>(@"
SELECT 1
FROM ([maciejadmin].[Rooms] r
LEFT JOIN [maciejadmin].[UsersRooms] ur
    ON r.Id = ur.RoomId)
INNER JOIN [maciejadmin].[Devices] d ON r.Id = d.RoomId
WHERE d.Id = @DeviceId AND (r.UserId = @UserId OR ur.UserId = @UserId)
", new { DeviceId = deviceId, UserId = userId });
    }

    public async Task<bool> UserHasWriteAccess(Guid deviceId, string userId)
    {
        using var conn = GetDbConnection();

        return await conn.ExecuteScalarAsync<bool>(@"
SELECT 1
FROM [maciejadmin].[Rooms] r
INNER JOIN [maciejadmin].[Devices] d ON r.Id = d.RoomId
WHERE d.Id = @DeviceId AND r.UserId = @UserId
", new { DeviceId = deviceId, UserId = userId });
    }
}
