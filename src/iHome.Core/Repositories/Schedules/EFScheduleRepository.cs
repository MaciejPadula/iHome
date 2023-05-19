using iHome.Core.Exceptions.SqlExceptions;
using iHome.Core.Models;
using iHome.Infrastructure.SQL.Contexts;
using iHome.Infrastructure.SQL.Models.RootTables;
using iHome.Shared.Logic;
using Microsoft.EntityFrameworkCore;

namespace iHome.Core.Repositories.Schedules;

public class EFScheduleRepository : IScheduleRepository
{
    private readonly SqlDataContext _sqlDataContext;

    public EFScheduleRepository(SqlDataContext sqlDataContext)
    {
        _sqlDataContext = sqlDataContext;
    }

    public async Task Add(string scheduleName, int hour, int minute, string userId)
    {
        await _sqlDataContext.Schedules.AddAsync(new Schedule
        {
            Name = scheduleName,
            UserId = userId,
            ActivationCron = CronHelper.CreateCronExpressions(0, hour, minute)
        });

        await _sqlDataContext.SaveChangesAsync();
    }

    public async Task<int> CountByUserId(string userId)
    {
        return await _sqlDataContext.Schedules.CountAsync(s => s.UserId == userId);
    }

    public async Task<ScheduleModel> GetById(Guid scheduleId)
    {
        return await _sqlDataContext.Schedules
            .Where(s => s.Id == scheduleId)
            .Select(s => new ScheduleModel(s))
            .FirstOrDefaultAsync() ?? throw new ScheduleNotFoundException();
    }

    public async Task<ScheduleModel> GetByIdWithDevices(Guid scheduleId)
    {
        return await _sqlDataContext.Schedules
            .Where(s => s.Id == scheduleId)
            .Include(s => s.ScheduleDevices)
            .ThenInclude(d => d.Device)
            .Select(s => new ScheduleModel(s))
            .FirstOrDefaultAsync() ?? throw new ScheduleNotFoundException();
    }

    public Task<List<ScheduleModel>> GetByUserId(string userId)
    {
        return _sqlDataContext.Schedules
            .Where(s => s.UserId == userId)
            .Select(s => new ScheduleModel(s))
            .ToListAsync();
    }

    public Task Remove(Guid scheduleId)
    {
        return ModifySchedule(scheduleId, (ctx, schedule) => ctx.Remove(schedule));
    }

    public Task UpdateTime(Guid scheduleId, int hour, int minute)
    {
        return ModifySchedule(scheduleId, (ctx, schedule) => schedule.ActivationCron = CronHelper.CreateCronExpressions(0, hour, minute));
    }

    private async Task ModifySchedule(Guid scheduleId, Action<SqlDataContext, Schedule> updater)
    {
        var schedule = await _sqlDataContext.Schedules
            .FirstOrDefaultAsync(d => d.Id == scheduleId) ?? throw new ScheduleNotFoundException();

        updater.Invoke(_sqlDataContext, schedule);

        await _sqlDataContext.SaveChangesAsync();
    }
}
