using iHome.Logic.ConfigProvider;
using iHome.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace iHome.Logic.Database
{
    public class ApplicationDbContext : DbContext
    {
        private readonly string _connectionString;
        public ApplicationDbContext(IConfigProvider configProvider)
        {
            _connectionString = configProvider.Configuration.AzureConnectionString;
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

