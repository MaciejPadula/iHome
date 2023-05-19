using iHome.Core.Logic.ActionValidators;
using iHome.Core.Models;
using iHome.Core.Repositories.Rooms;
using iHome.Core.Services.Users;

namespace iHome.Core.Services;

public interface IRoomService
{
    Task AddRoom(string roomName, string userId);
    Task<IEnumerable<RoomModel>> GetRooms(string userId);
    Task RemoveRoom(Guid roomId, string userId);

    Task ShareRoom(Guid roomId, string userId, string callerUserId);
    Task<IEnumerable<User>> GetRoomUsers(Guid roomId, string userId);
    Task UnshareRoom(Guid roomId, string userId, string callerUserId);
}

public class RoomService : IRoomService
{
    private readonly IRoomActionValidator _roomActionValidator;
    private readonly IUserService _userService;
    private readonly IRoomRepository _roomRepository;
    private readonly IUserRoomRepository _userRoomRepository;

    public RoomService(IRoomActionValidator roomActionValidator, IUserService userService, IRoomRepository roomRepository, IUserRoomRepository userRoomRepository)
    {
        _roomActionValidator = roomActionValidator;
        _userService = userService;
        _roomRepository = roomRepository;
        _userRoomRepository = userRoomRepository;
    }

    public Task AddRoom(string roomName, string userId)
    {
        return _roomRepository.Add(roomName, userId);
    }

    public async Task<IEnumerable<RoomModel>> GetRooms(string userId)
    {
        var rooms = await _roomRepository.GetByUserId(userId);

        foreach (var room in rooms)
        {
            if (room.User == null) continue;
            var newUser = await _userService.GetUserById(room.User.Id);
            if (newUser == null) continue;

            room.User = newUser;
        }

        return rooms;
    }

    public async Task RemoveRoom(Guid roomId, string userId)
    {
        _roomActionValidator.ValidateWriteAccessAndThrow(roomId, userId);

        await _roomRepository.Remove(roomId);
    }

    public async Task ShareRoom(Guid roomId, string userId, string callerUserId)
    {
        _roomActionValidator.ValidateShareRoomAndThrow(roomId, userId, callerUserId);

        await _userRoomRepository.AddUserRoom(roomId, userId);
    }

    public async Task<IEnumerable<User>> GetRoomUsers(Guid roomId, string userId)
    {
        _roomActionValidator.ValidateReadAccessAndThrow(roomId, userId);

        var usersIds = await _userRoomRepository.GetRoomUsersIds(roomId);

        var users = new List<User>();

        foreach (var uid in usersIds)
        {
            var response = await _userService.GetUserById(uid);
            if (response == null) continue;

            users.Add(response);
        }

        return users;
    }

    public async Task UnshareRoom(Guid roomId, string userId, string callerUserId)
    {
        _roomActionValidator.ValidateWriteAccessAndThrow(roomId, callerUserId);

        await _userRoomRepository.RemoveUserRoom(roomId, userId);
    }
}
