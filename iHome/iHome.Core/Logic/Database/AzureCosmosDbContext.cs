using iHome.Core.Models.Application;
using iHome.Core.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;

namespace iHome.Core.Logic.Database
{
    public class AzureCosmosDbContext : DbContext, IDatabaseContext
    {
        public DbSet<TDevice> Devices => Set<TDevice>();
        public DbSet<TRoom> Rooms => Set<TRoom>();
        public DbSet<TDeviceToConfigure> DevicesToConfigure => Set<TDeviceToConfigure>();
        public DbSet<TUsersRooms> UsersRooms => Set<TUsersRooms>();
        public DbSet<TBills> Bills => Set<TBills>();
        private readonly string _connectionString;

        public AzureCosmosDbContext(IOptions<ApplicationSettings> options)
        {
            _connectionString = options.Value.AzureConnectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseCosmos("", "");
        }

        EntityEntry IDatabaseContext.Entry(object entity)
        {
            return Entry(entity);
        }

        int IDatabaseContext.SaveChanges()
        {
            return SaveChanges();
        }
    }
}
