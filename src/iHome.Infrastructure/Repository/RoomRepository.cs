using iHome.Microservices.RoomsManagement.Contract;
using iHome.Model;
using iHome.Repository;

namespace iHome.Infrastructure.Repository;

internal class RoomRepository : IRoomRepository
{
    private readonly IRoomManagementService _roomManagementService;

    public RoomRepository(IRoomManagementService roomManagementService)
    {
        _roomManagementService = roomManagementService;
    }

    public Task Add(string roomName, string userId)
    {
        return _roomManagementService.AddRoom(new()
        {
            RoomName = roomName,
            UserId = userId
        });
    }

    public async Task<IEnumerable<RoomDto>> GetUserRooms(string userId)
    {
        var response = await _roomManagementService.GetRooms(new()
        {
            UserId = userId
        });

        return response?
            .Rooms?
            .Select(r => new RoomDto
            {
                Id = r.Id,
                Name = r.Name,
                UserId = r.UserId
            }) ?? Enumerable.Empty<RoomDto>();
    }

    public Task Remove(Guid roomId)
    {
        return _roomManagementService.RemoveRoom(new()
        {
            RoomId = roomId
        });
    }
}
