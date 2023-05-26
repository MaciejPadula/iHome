using iHome.Core.Models;

namespace iHome.Microservices.RoomsManagement.Infrastructure.Repositories;

public interface IRoomRepository
{
    Task Add(string name, string userId);
    Task<RoomModel?> GetRoomById(Guid roomId);
    Task<IEnumerable<RoomModel>> GetRoomsByUserId(string userId);
    Task<RoomModel?> GetRoomByRoomIdAndUserId(Guid roomId, string userId);
    Task Update(Guid roomId, string name);
    Task Remove(Guid roomId);
}