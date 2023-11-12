using iHome.Model;

namespace iHome.Repository;

public interface IRoomRepository
{
    Task Add(string roomName, string userId);
    Task<IEnumerable<RoomDto>> GetUserRooms(string userId);
    Task Remove(Guid roomId);
}
