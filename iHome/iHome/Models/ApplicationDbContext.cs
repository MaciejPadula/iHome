using iHome.Models.Database;
using Microsoft.EntityFrameworkCore;
namespace iHome.Models
{
    public class ApplicationdbContext : DbContext
    {
        private readonly string connectionString;
        public ApplicationdbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(connectionString);
        }
        public DbSet<TDevice> Devices { get; set; }
        public DbSet<TRoom> Rooms { get; set; }
    }
}

