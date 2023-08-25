using Dapper;
using iHome.Infrastructure.Sql.Factories;
using iHome.Infrastructure.Sql.Repositories;
using iHome.Microservices.Authorization.Domain.Repositories;

namespace iHome.Microservices.Authorization.Infrastructure.Repositories;

public class DapperRoomRepository : RepositoryBase, IRoomRepository
{
    public DapperRoomRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public async Task<bool> UserHasReadAccess(Guid roomId, string userId)
    {
        using var conn = GetDbConnection();

        return await conn.ExecuteScalarAsync<bool>(@"
SELECT 1
FROM [maciejadmin].[Rooms] r
LEFT JOIN [maciejadmin].[UsersRooms] ur
    ON r.Id = ur.RoomId
WHERE r.Id = @RoomId AND (r.UserId = @UserId OR ur.UserId = @UserId)
", new { RoomId = roomId, UserId = userId });
    }

    public async Task<bool> UserHasWriteAccess(Guid roomId, string userId)
    {
        using var conn = GetDbConnection();

        return await conn.ExecuteScalarAsync<bool>(@"
SELECT 1
FROM [maciejadmin].[Rooms] r
WHERE r.Id = @RoomId AND r.UserId = @UserId
", new { RoomId = roomId, UserId = userId });
    }
}
