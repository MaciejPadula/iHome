using Dapper;
using iHome.Infrastructure.Sql.Factories;
using iHome.Infrastructure.Sql.Repositories;
using iHome.Microservices.Schedules.Contract.Models;

namespace iHome.Microservices.Schedules.Infrastructure.Repositories;

public class DapperScheduleRepository : RepositoryBase, IScheduleRepository
{
    public DapperScheduleRepository(IDbConnectionFactory connectionFactory) 
        : base(connectionFactory)
    {
    }

    public async Task Add(string scheduleName, int hour, int minute, string userId)
    {
        using var conn = GetDbConnection();

        await conn.ExecuteAsync(@"
INSERT INTO [maciejadmin].[Schedules]
    (Id, Name, Hour, Minute, Modified, UserId)
VALUES
    (@Id, @Name, @Hour, @Minute, @Modified, @UserId)
        ", new
        {
            Id = Guid.NewGuid(),
            Name = scheduleName,
            Hour = hour,
            Minute = minute,
            UserId = userId,
            Modified = DateTime.UtcNow
        });
    }

    public async Task<int> CountByUserId(string userId)
    {
        using var conn = GetDbConnection();

        return await conn.ExecuteScalarAsync<int>(@"
SELECT Count(Id)
FROM [maciejadmin].[Schedules]
WHERE UserId = @UserId
", new { UserId = userId });
    }

    public async Task<ScheduleModel> GetById(Guid scheduleId)
    {
        using var conn = GetDbConnection();

        return await conn.QuerySingleAsync<ScheduleModel>(@$"
SELECT
    Id as {nameof(ScheduleModel.Id)},
    Name as {nameof(ScheduleModel.Name)},
    Hour as {nameof(ScheduleModel.Hour)},
    Minute as {nameof(ScheduleModel.Minute)},
    UserId as {nameof(ScheduleModel.UserId)}
FROM [maciejadmin].[Schedules]
WHERE Id = @Id
", new { Id = scheduleId });
    }

    public async Task<IEnumerable<ScheduleModel>> GetByUserId(string userId)
    {
        using var conn = GetDbConnection();

        return await conn.QueryAsync<ScheduleModel>(@$"
SELECT
    Id as {nameof(ScheduleModel.Id)},
    Name as {nameof(ScheduleModel.Name)},
    Hour as {nameof(ScheduleModel.Hour)},
    Minute as {nameof(ScheduleModel.Minute)},
    UserId as {nameof(ScheduleModel.UserId)}
FROM [maciejadmin].[Schedules]
WHERE UserId = @UserId
", new { UserId = userId });
    }

    public async Task Remove(Guid scheduleId)
    {
        using var conn = GetDbConnection();

        await conn.ExecuteAsync(@"
DELETE FROM [maciejadmin].[Schedules]
WHERE Id = @ScheduleId
        ", new { ScheduleId = scheduleId });
    }

    public async Task UpdateTime(Guid scheduleId, int hour, int minute)
    {
        using var conn = GetDbConnection();

        await conn.ExecuteAsync(@"
UPDATE [maciejadmin].[Schedules]
SET 
    Hour = @Hour,
    Minute = @Minute,
    Modified = @Modified
WHERE Id = @Id
        ", new { Id = scheduleId, Hour = hour, Minute = minute, Modified = DateTime.UtcNow });
    }
}
