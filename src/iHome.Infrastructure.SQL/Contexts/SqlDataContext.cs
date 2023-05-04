using iHome.Infrastructure.SQL.Models.ChildrenTables;
using iHome.Infrastructure.SQL.Models.ConnectionTables;
using iHome.Infrastructure.SQL.Models.RootTables;
using Microsoft.EntityFrameworkCore;

namespace iHome.Infrastructure.SQL.Contexts;

public class SqlDataContext : DbContext
{
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<UserRoom> UserRoom => Set<UserRoom>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<Widget> Widgets => Set<Widget>();
    public DbSet<WidgetDevice> WidgetsDevices => Set<WidgetDevice>();
    public DbSet<Schedule> Schedules => Set<Schedule>();
    public DbSet<ScheduleDevice> ScheduleDevices => Set<ScheduleDevice>();
    public DbSet<ScheduleRunHistory> SchedulesRunHistory => Set<ScheduleRunHistory>();

    public SqlDataContext(DbContextOptions<SqlDataContext> options)
        : base(options)
    { }
}
