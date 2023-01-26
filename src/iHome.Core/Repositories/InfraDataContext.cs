using iHome.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace iHome.Core.Repositories;
public class InfraDataContext : DbContext
{
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<SharedRoom> SharedRooms => Set<SharedRoom>();

    public InfraDataContext(DbContextOptions<InfraDataContext> options)
        : base(options)
    { }
}
