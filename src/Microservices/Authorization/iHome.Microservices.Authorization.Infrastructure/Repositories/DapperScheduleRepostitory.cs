using Dapper;
using iHome.Infrastructure.Sql.Factories;
using iHome.Infrastructure.Sql.Repositories;
using iHome.Microservices.Authorization.Domain.Repositories;

namespace iHome.Microservices.Authorization.Infrastructure.Repositories;

public class DapperScheduleRepostitory : RepositoryBase, IScheduleRepostitory
{
    public DapperScheduleRepostitory(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public async Task<bool> UserHasAccess(Guid scheduleId, string userId)
    {
        using var conn = GetDbConnection();

        return await conn.ExecuteScalarAsync<bool>(@"
SELECT 1
FROM [maciejadmin].[Schedules] r
WHERE r.Id = @ScheduleId AND r.UserId = @UserId
", new { ScheduleId = scheduleId, UserId = userId });
    }
}
