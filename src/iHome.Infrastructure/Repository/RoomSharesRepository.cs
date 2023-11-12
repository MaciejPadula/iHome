using iHome.Microservices.RoomsManagement.Contract;
using iHome.Repository;

namespace iHome.Infrastructure.Repository;

internal class RoomSharesRepository : IRoomSharesRepository
{
    private readonly IRoomSharingService _roomSharingService;

    public RoomSharesRepository(IRoomSharingService roomSharingService)
    {
        _roomSharingService = roomSharingService;
    }

    public async Task Add(Guid roomId, string userId)
    {
        await _roomSharingService.ShareRoomToUser(new()
        {
            RoomId = roomId,
            SubjectUserId = userId
        });
    }

    public async Task<IEnumerable<string>> GetRoomUsersIds(Guid roomId)
    {
        var response = await _roomSharingService.GetRoomUserIds(new()
        {
            RoomId = roomId
        });

        return response?.UsersIds ?? Enumerable.Empty<string>();
    }

    public async Task Remove(Guid roomId, string userId)
    {
        await _roomSharingService.UnshareRoomFromUser(new()
        {
            RoomId = roomId,
            SubjectUserId = userId
        });
    }
}
