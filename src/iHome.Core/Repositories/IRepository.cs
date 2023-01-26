using iHome.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace iHome.Core.Repositories;
public interface IRepository
{
    DbSet<Room> Rooms { get; }
    DbSet<SharedRoom> SharedRooms { get; }

    void SaveChanges();
}
