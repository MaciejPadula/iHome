using iHome.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace iHome.Core.Repositories;

public class CachedRepository : IRepository
{
    private readonly IMemoryCache _memoryCache;
    private readonly InfraDataContext _infraDb;

    public CachedRepository(IMemoryCache memoryCache, InfraDataContext infraDb)
    {
        _memoryCache = memoryCache;
        _infraDb = infraDb;
    }

    public DbSet<Room> Rooms => _infraDb.Rooms;

    public DbSet<SharedRoom> SharedRooms => _infraDb.SharedRooms;

    public void SaveChanges()
    {
        _infraDb.SaveChanges();
    }
}
