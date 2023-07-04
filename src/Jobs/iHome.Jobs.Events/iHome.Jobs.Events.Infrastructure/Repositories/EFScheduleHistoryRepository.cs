﻿using iHome.Jobs.Events.Infrastructure.Contexts;
using iHome.Jobs.Events.Infrastructure.Models;

namespace iHome.Jobs.Events.Infrastructure.Repositories;

public class EFScheduleHistoryRepository : IScheduleHistoryRepository
{
    private readonly SqlDataContext _context;

    public EFScheduleHistoryRepository(SqlDataContext context)
    {
        _context = context;
    }

    public async Task AddRunnedSchedules(IEnumerable<Guid> scheduleIds, DateTime runDate)
    {
        foreach (var scheduleId in scheduleIds)
        {
            _context.Add(new ScheduleRunHistory
            {
                Id = Guid.NewGuid(),
                ScheduleId = scheduleId,
                RunDate = runDate
            });
        }

        await _context.SaveChangesAsync();
    }
}
