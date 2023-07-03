using Dapper;
using iHome.Infrastructure.Sql.Factories;
using iHome.Infrastructure.Sql.Repositories;
using iHome.Microservices.Devices.Contract.Models;

namespace iHome.Microservices.Schedules.Infrastructure.Repositories
{
    public class DapperDeviceRepository : RepositoryBase, IDeviceRepository
    {
        public DapperDeviceRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<IEnumerable<Guid>> GetDevicesForUserByTypes(string userId, IEnumerable<DeviceType> deviceTypes)
        {
            using var conn = GetDbConnection();

            return await conn.QueryAsync<Guid>(@"
SELECT d.Id
FROM [maciejadmin].[Devices] d
INNER JOIN [maciejadmin].[Rooms] r
ON  d.RoomId = r.Id
WHERE r.UserId = @UserId AND d.Type IN @DeviceTypes
", new { UserId = userId, DeviceTypes = deviceTypes });
        }
    }
}
