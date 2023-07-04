using iHome.Core.Models;
using iHome.Microservices.RoomsManagement.Contract;
using iHome.Microservices.UsersApi.Contract;
using iHome.Microservices.UsersApi.Contract.Models;

namespace iHome.Core.Services;

public interface IRoomService
{
    Task<List<RoomDTO>> GetRooms(string userId);
    Task<List<User>> GetRoomUsers(Guid roomId, string userId);
}

public class RoomService : IRoomService
{
    private readonly IRoomManagementService _roomManagementService;
    private readonly IRoomSharingService _roomSharingService;
    private readonly IUserManagementService _userManagementService;

    public RoomService(IRoomManagementService roomManagementService, IRoomSharingService roomSharingService, IUserManagementService userManagementService)
    {
        _roomManagementService = roomManagementService;
        _roomSharingService = roomSharingService;
        _userManagementService = userManagementService;
    }

    public async Task<List<RoomDTO>> GetRooms(string userId)
    {
        var response = await _roomManagementService.GetRooms(new()
        {
            UserId = userId
        });

        var responseRooms = response?.Rooms?.ToList() ?? Enumerable.Empty<RoomModel>().ToList();

        var rooms = new List<RoomDTO>();

        foreach (var room in responseRooms)
        {
            var usr = await _userManagementService.GetUserById(new() { UserId = room.UserId });
            rooms.Add(new RoomDTO(room, usr?.User ?? new User { Id = room.UserId }));
        }

        return rooms;
    }

    public async Task<List<User>> GetRoomUsers(Guid roomId, string userId)
    {
        var userIds = await _roomSharingService.GetRoomUserIds(new()
        {
            RoomId = roomId,
            UserId = userId
        });

        var users = new List<User>();

        foreach (var uid in userIds.UsersIds)
        {
            var usr = await _userManagementService.GetUserById(new() { UserId = uid });
            if (usr?.User == null)
            {
                continue;
            }
            users.Add(usr.User);
        }

        return users.OrderBy(u => u.Name).ToList();
    }
}
