using iHome.Models.Database;
using Microsoft.EntityFrameworkCore;
namespace iHome.Logic
{
    public class ApplicationdbContext : DbContext
    {
        private readonly string _connectionString;
        public ApplicationdbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(_connectionString);
        }
        public DbSet<TDevice>? Devices { get; set; }
        public DbSet<TRoom>? Rooms { get; set; }
        public DbSet<TDeviceToConfigure>? DevicesToConfigure { get; set; }
        public DbSet<TUsersRooms>? UsersRooms { get; set; }
    }
}

