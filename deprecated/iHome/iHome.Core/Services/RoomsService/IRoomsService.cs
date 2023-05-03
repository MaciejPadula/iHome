using iHome.Core.Models.ApiRooms;

namespace iHome.Core.Services.RoomsService
{
    public interface IRoomsService
    {
        Task<List<Room>> GetRooms(string uuid);
        Task AddRoom(string roomName, string roomDescription, string uuid);
        Task RemoveRoom(Guid roomId);

        Task AddUserRoomConstraint(Guid roomId, string uuid);
        Task RemoveUserRoomConstraint(Guid roomId, string uuid, string masterUuid);
        Task<List<string>> GetRoomUserIds(Guid roomId);
    }
}
