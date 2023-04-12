using iHome.Infrastructure.SQL.Models;
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

    public SqlDataContext(DbContextOptions<SqlDataContext> options)
        : base(options)
    { }
}
