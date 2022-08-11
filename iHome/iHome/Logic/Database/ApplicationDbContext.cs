using iHome.Models.Application;
using iHome.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace iHome.Logic.Database
{
    public class ApplicationDbContext : DbContext
    {
        private readonly string _connectionString;
        public ApplicationDbContext(IOptions<ApplicationSettings> options)
        {
            _connectionString = options.Value.AzureConnectionString;
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

