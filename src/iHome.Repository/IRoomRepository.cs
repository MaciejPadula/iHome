using iHome.Model;

namespace iHome.Repository;

public interface IRoomRepository
{
    Task Add(RoomDto room);
    Task<IEnumerable<RoomDto>> GetUserRooms(string userId);
    Task Remove(Guid roomId);
}
