using iHome.Core.Models;
using iHome.Infrastructure.SQL.Models;

namespace iHome.Core.Services.Rooms;
public interface IRoomService
{
    Task<bool> UserCanAccessRoom(Guid roomId, string userId);

    Task AddRoom(string roomName, string userId);
    Task<IEnumerable<Room>> GetRooms(string userId);
    Task<IEnumerable<Room>> GetRoomsWithDevices(string userId);
    Task RemoveRoom(Guid roomId, string userId);

    Task ShareRoom(Guid roomId, string userId, string callerUserId);
    Task<IEnumerable<UserRoom>> GetRoomUsers(Guid roomId, string userId);
    Task UnshareRoom(Guid roomId, string userId, string callerUserId);
}
