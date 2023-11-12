using iHome.Model;

namespace iHome.Features.RoomSharing;

public interface IRoomSharingService
{
    Task ShareRoom(Guid roomId, string userId, string callerId);
    Task UnshareRoom(Guid roomId, string userId, string callerId);
    Task<IEnumerable<UserDto>> GetRoomUsers(Guid roomId, string userId);
}
