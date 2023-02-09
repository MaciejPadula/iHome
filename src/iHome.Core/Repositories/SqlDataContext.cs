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
        return Rooms
            .Where(room => room.UserId == userId)
            .GroupJoin(
                UserRoom.Where(sharedRoom => sharedRoom.UserId == userId),
                room => room.Id,
                sharedRoom => sharedRoom.RoomId,
                (room, sharedRoom) => room
            );
    }
}
