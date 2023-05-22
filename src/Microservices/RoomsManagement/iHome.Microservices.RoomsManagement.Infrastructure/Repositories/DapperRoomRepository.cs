using Dapper;
using iHome.Core.Models;

namespace iHome.Microservices.RoomsManagement.Infrastructure.Repositories;

public class DapperRoomRepository : IRoomRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DapperRoomRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task Add(string name, string userId)
    {
        using var conn = _connectionFactory.GetConnection();

        await conn.ExecuteAsync(@"
INSERT INTO [maciejadmin].[Rooms]
    (Id, Name, UserId)
VALUES
    (@Id, @Name, @UserId)
        ", new { Id = Guid.NewGuid(), Name = name, UserId = userId });
    }

    public async Task<RoomModel?> GetRoomById(Guid roomId)
    {
        using var conn = _connectionFactory.GetConnection();

        return await conn.QuerySingleOrDefaultAsync<RoomModel>(@$"
SELECT
    Id as {nameof(RoomModel.Id)},
    Name as {nameof(RoomModel.Name)},
    UserId as {nameof(RoomModel.UserId)}
FROM [maciejadmin].[Rooms]
WHERE Id = @Id
", new { Id = roomId });
    }

    public async Task<RoomModel?> GetRoomByRoomIdAndUserId(Guid roomId, string userId)
    {
        using var conn = _connectionFactory.GetConnection();

        return await conn.QuerySingleOrDefaultAsync<RoomModel>(@$"
SELECT
    Id as {nameof(RoomModel.Id)},
    Name as {nameof(RoomModel.Name)},
    UserId as {nameof(RoomModel.UserId)}
FROM [maciejadmin].[Rooms]
WHERE Id = @Id AND UserId = @UserId
", new { Id = roomId, UserId = userId });
    }

    public async Task<IEnumerable<RoomModel>> GetRoomsByUserId(string userId)
    {
        using var conn = _connectionFactory.GetConnection();

        return await conn.QueryAsync<RoomModel>(@$"
SELECT DISTINCT
    r.Id as {nameof(RoomModel.Id)},
    r.Name as {nameof(RoomModel.Name)},
    r.UserId as {nameof(RoomModel.UserId)}
FROM [maciejadmin].[Rooms] r
LEFT JOIN [maciejadmin].[UsersRooms] ur
    ON r.Id = ur.RoomId
WHERE r.UserId = @UserId OR ur.UserId = @UserId
ORDER BY r.Name
", new { UserId = userId });
    }

    public async Task Remove(Guid roomId)
    {
        using var conn = _connectionFactory.GetConnection();

        await conn.ExecuteAsync(@"
DELETE FROM [maciejadmin].[Rooms]
WHERE Id = @RoomId
        ", new { RoomId = roomId });
    }

    public async Task Update(Guid roomId, string name)
    {
        using var conn = _connectionFactory.GetConnection();

        await conn.ExecuteAsync(@"
UPDATE [maciejadmin].[Rooms]
SET Name = @Name
WHERE RoomId = @RoomId
        ", new { RoomId = roomId, Name = name });
    }
}
