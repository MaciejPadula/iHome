using iHome.Core.Models;

namespace iHome.Core.Services.Rooms;
public interface IRoomService
{
    bool UserCanAccessRoom(Guid roomId, string userId);

    void AddRoom(string roomName, string userId);
    IEnumerable<Room> GetRooms(string userId);
    IEnumerable<Room> GetRoomsWithDevices(string userId);
    void RemoveRoom(Guid roomId, string userId);

    void ShareRoom(Guid roomId, string userId, string callerUserId);
    void UnshareRoom(Guid roomId, string userId, string callerUserId);
}
