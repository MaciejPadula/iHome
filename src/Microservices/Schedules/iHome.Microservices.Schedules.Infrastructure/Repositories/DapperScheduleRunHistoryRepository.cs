using Dapper;
using iHome.Infrastructure.Sql.Factories;
using iHome.Infrastructure.Sql.Repositories;

namespace iHome.Microservices.Schedules.Infrastructure.Repositories;

public class DapperScheduleRunHistoryRepository : RepositoryBase, IScheduleRunHistoryRepository
{
    public DapperScheduleRunHistoryRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public async Task<bool> ScheduleRunned(Guid scheduleId, DateTime startOfToday)
    {
        using var conn = GetDbConnection();

        return await conn.ExecuteScalarAsync<bool>(@"
SELECT 1 
FROM [maciejadmin].[SchedulesRunHistory]
WHERE ScheduleId = @ScheduleId AND RunDate > @StartOfDay
", new { ScheduleId = scheduleId, StartOfDay = startOfToday });
    }
}
