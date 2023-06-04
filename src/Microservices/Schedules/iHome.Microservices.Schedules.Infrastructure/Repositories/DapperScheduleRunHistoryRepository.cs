using Dapper;

namespace iHome.Microservices.Schedules.Infrastructure.Repositories;

public class DapperScheduleRunHistoryRepository : IScheduleRunHistoryRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DapperScheduleRunHistoryRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> ScheduleRunned(Guid scheduleId, DateTime startOfToday)
    {
        using var conn = _connectionFactory.GetConnection();

        return await conn.ExecuteScalarAsync<bool>(@"
SELECT 1 
FROM [maciejadmin].[SchedulesRunHistory]
WHERE ScheduleId = @ScheduleId AND RunDate > @StartOfDay
", new { ScheduleId = scheduleId, StartOfDay = startOfToday });
    }
}
