using iHome.Core.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace iHome.Core.Logic.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TDevice> Devices => Set<TDevice>();
        public DbSet<TRoom> Rooms => Set<TRoom>();
        public DbSet<TDeviceToConfigure> DevicesToConfigure => Set<TDeviceToConfigure>();
        public DbSet<TUserRoom> UsersRooms => Set<TUserRoom>();
    }
}

