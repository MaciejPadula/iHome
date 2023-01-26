using iHome.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace iHome.Core.Repositories;
public class CachedRepository : IRepository
{
    private readonly IMemoryCache _memoryCache;
    private readonly InfraDataContext _infraDb;
    private readonly DevicesDataContext _devicesDb;

    public CachedRepository(IMemoryCache memoryCache, InfraDataContext infraDb, DevicesDataContext devicesDb)
    {
        _memoryCache = memoryCache;
        _infraDb = infraDb;
        _devicesDb = devicesDb;
    }

    public DbSet<Room> Rooms => _infraDb.Rooms;

    public DbSet<SharedRoom> SharedRooms => _infraDb.SharedRooms;

    public DbSet<Device> Devices => _devicesDb.Devices;

    public void SaveChanges()
    {
        _infraDb.SaveChanges();
        _devicesDb.SaveChanges();
    }
}
