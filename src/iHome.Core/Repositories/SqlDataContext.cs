using iHome.Core.Models;
using iHome.Devices.Contract.Models;
using Microsoft.EntityFrameworkCore;

namespace iHome.Core.Repositories;
public class SqlDataContext : DbContext
{
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<UserRoom> UserRoom => Set<UserRoom>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<Widget> Widgets => Set<Widget>();
    public DbSet<WidgetDevice> WidgetsDevices => Set<WidgetDevice>();

    public SqlDataContext(DbContextOptions<SqlDataContext> options)
        : base(options)
    { }

    public IQueryable<Room> GetUsersRooms(string userId)
    {
        var rooms = Rooms.Where(r => r.UserId == userId);

        var sharedRooms = UserRoom
            .Where(r => r.UserId == userId)
            .Include(r => r.Room)
            .Select(r => r.Room);

        return rooms.Concat(sharedRooms);
    }
}
