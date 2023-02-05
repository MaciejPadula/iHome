using iHome.Core.Models;
using iHome.Devices.Contract.Models;
using Microsoft.EntityFrameworkCore;

namespace iHome.Core.Repositories;
public class SqlDataContext : DbContext
{
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<SharedRoom> SharedRooms => Set<SharedRoom>();
    public DbSet<Device> Devices => Set<Device>();

    public SqlDataContext(DbContextOptions<SqlDataContext> options)
        : base(options)
    { }

    public IQueryable<Room> GetUsersRooms(Guid userId)
    {
        return Rooms
            .Where(room => room.UserId == userId)
            .GroupJoin(
                SharedRooms.Where(sharedRoom => sharedRoom.UserId == userId),
                room => room.Id,
                sharedRoom => sharedRoom.RoomId,
                (room, sharedRoom) => room
            );
    }
}
