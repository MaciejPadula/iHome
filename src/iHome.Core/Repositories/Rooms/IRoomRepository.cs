using iHome.Core.Models;

namespace iHome.Core.Repositories.Rooms;

public interface IRoomRepository
{
    Task Add(string name, string userId);
    Task<List<RoomModel>> GetById(Guid roomId);
    Task<List<RoomModel>> GetByUserId(string userId);
    Task<List<RoomModel>> GetByRoomIdAndUserId(Guid roomId, string userId);
    Task Update(Guid roomId, string name);
    Task Remove(Guid roomId);
}
