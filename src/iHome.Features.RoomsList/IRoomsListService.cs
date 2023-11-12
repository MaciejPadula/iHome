using iHome.Model;

namespace iHome.Features.RoomsList;

public interface IRoomsListService
{
    Task<IEnumerable<RoomDto>> GetUserRooms(string userId);
    Task AddRoom(string name, string userId);
    Task RemoveRoom(Guid id, string userId);
}

