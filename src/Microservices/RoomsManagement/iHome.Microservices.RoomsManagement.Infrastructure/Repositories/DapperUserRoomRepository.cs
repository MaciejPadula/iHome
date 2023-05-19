using Dapper;
using iHome.Core.Models;

namespace iHome.Microservices.RoomsManagement.Infrastructure.Repositories
{
    public class DapperUserRoomRepository : IUserRoomRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DapperUserRoomRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task AddUserRoom(Guid roomId, string userId)
        {
            using var conn = _connectionFactory.GetConnection();

            await conn.ExecuteAsync(@"
INSERT INTO [maciejadmin].[UsersRooms]
    (Id, RoomId, UserId)
VALUES
    (@Id, @RoomId, @UserId)
        ", new { Id = Guid.NewGuid(), RoomId = roomId, UserId = userId });
        }

        public async Task<IEnumerable<string>> GetRoomUsersIds(Guid roomId)
        {
            using var conn = _connectionFactory.GetConnection();

            return await conn.QueryAsync<string>(@"
SELECT UserId
FROM [maciejadmin].[UsersRooms]
WHERE RoomId = @RoomId
", new { RoomId = roomId });
        }

        public async Task RemoveUserRoom(Guid roomId, string userId)
        {
            using var conn = _connectionFactory.GetConnection();

            await conn.ExecuteAsync(@"
DELETE FROM [maciejadmin].[UsersRooms]
WHERE RoomId = @RoomId AND UserId = @UserId
        ", new { RoomId = roomId, UserId = userId });
        }
    }
}
