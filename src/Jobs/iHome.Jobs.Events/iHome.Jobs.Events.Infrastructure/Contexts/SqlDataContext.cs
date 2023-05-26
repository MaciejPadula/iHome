using iHome.Jobs.Events.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace iHome.Jobs.Events.Infrastructure.Contexts;

public class SqlDataContext : DbContext
{
    public DbSet<Schedule> Schedules => Set<Schedule>();
    public DbSet<ScheduleDevice> ScheduleDevices => Set<ScheduleDevice>();
    public DbSet<ScheduleRunHistory> SchedulesRunHistory => Set<ScheduleRunHistory>();

    public SqlDataContext(DbContextOptions<SqlDataContext> options)
        : base(options)
    { }
}
