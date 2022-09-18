using iHome.Core.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace iHome.Core.Logic.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var converter = new ValueConverter<Guid, string>(
                v => v.ToString(),
                v => Guid.Parse(v));

            modelBuilder
                .Entity<TRoom>()
                .Property(e => e.RoomId)
                .HasConversion(converter);

            modelBuilder
                .Entity<TDevice>()
                .Property(e => e.RoomId)
                .HasConversion(converter);

            modelBuilder
                .Entity<TUserRoom>()
                .Property(e => e.RoomId)
                .HasConversion(converter);
        }

        public DbSet<TDevice> Devices => Set<TDevice>();
        public DbSet<TRoom> Rooms => Set<TRoom>();
        public DbSet<TDeviceToConfigure> DevicesToConfigure => Set<TDeviceToConfigure>();
        public DbSet<TUserRoom> UsersRooms => Set<TUserRoom>();
    }
}

