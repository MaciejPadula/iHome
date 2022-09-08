using iHome.Core.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace iHome.Core.Logic.Database
{
    public interface IDatabaseContext
    {
        DbSet<TDevice> Devices { get; }
        DbSet<TRoom> Rooms { get; }
        DbSet<TDeviceToConfigure> DevicesToConfigure { get; }
        DbSet<TUsersRooms> UsersRooms { get; }
        DbSet<TBills> Bills { get; }

        int SaveChanges();
        EntityEntry Entry(object entity);
    }
}
