using iHome.Core.Models.ApiRooms;

namespace iHome.Core.Services.RoomsService
{
    public interface IRoomsService
    {
        Task<List<Room>> GetRooms(string uuid);
        Task AddRoom(string roomName, string roomDescription, string uuid);
        Task RemoveRoom(int roomId);

        Task AddUserRoomConstraint(int roomId, string uuid);
        Task RemoveUserRoomConstraint(int roomId, string uuid, string masterUuid);
        Task<List<string>> GetRoomUserIds(int roomId);
    }
}
