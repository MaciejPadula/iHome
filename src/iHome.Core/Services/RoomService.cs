using iHome.Core.Helpers;
using iHome.Core.Logic.RoomDtoList;
using iHome.Core.Models;
using iHome.Core.Services.Validation;
using iHome.Microservices.RoomsManagement.Contract;
using iHome.Microservices.RoomsManagement.Contract.Models.Request;
using iHome.Microservices.UsersApi.Contract;
using iHome.Microservices.UsersApi.Contract.Models;

namespace iHome.Core.Services;

public interface IRoomService
{
    Task AddRoom(string roomName, string userId);
    Task<List<RoomDto>> GetRooms(string userId);
    Task<List<User>> GetRoomUsers(Guid roomId, string userId);
    Task RemoveRoom(Guid roomId, string userId);

    Task ShareRoom(Guid roomId, string userId, string callerId);
    Task UnshareRoom(Guid roomId, string userId, string callerId);
}

public class RoomService : IRoomService
{
    private readonly IRoomManagementService _roomManagementService;
    private readonly IRoomSharingService _roomSharingService;
    private readonly IUserManagementService _userManagementService;

    private readonly IValidationService _validationService;
    private readonly IRoomDtoListBuilder _roomDtoListBuilder;

    public RoomService(IRoomManagementService roomManagementService,
        IRoomSharingService roomSharingService,
        IUserManagementService userManagementService,
        IValidationService validationService,
        IRoomDtoListBuilder roomDtoListBuilder)
    {
        _roomManagementService = roomManagementService;
        _roomSharingService = roomSharingService;
        _userManagementService = userManagementService;
        _validationService = validationService;
        _roomDtoListBuilder = roomDtoListBuilder;
    }

    public async Task AddRoom(string roomName, string userId)
    {
        await _roomManagementService.AddRoom(new()
        {
            RoomName = roomName,
            UserId = userId
        });
    }

    public async Task<List<RoomDto>> GetRooms(string userId)
    {
        var response = await _roomManagementService.GetRooms(new()
        {
            UserId = userId
        });

        var rooms = response?.Rooms?.ToList() ?? Enumerable.Empty<RoomModel>().ToList();

        var users = await _userManagementService.GetUsersByIds(new()
        {
            Ids = rooms.Select(r => r.UserId).Distinct()
        });

        return await _roomDtoListBuilder.Build(
            rooms, users?.Users ?? new());
    }

    public async Task<List<User>> GetRoomUsers(Guid roomId, string userId)
    {
        var request = new GetRoomUserIdsRequest
        {
            RoomId = roomId,
            UserId = userId
        };

        var userIds = await _validationService.Validate(roomId, userId, ValidatorType.RoomWrite, () => _roomSharingService.GetRoomUserIds(request));

        var users = await _userManagementService.GetUsersByIds(new()
        {
            Ids = userIds?.UsersIds ?? Enumerable.Empty<string>()
        });

        return users?.Users?
            .Select(kv => kv.Value)?
            .OrderBy(u => u.Name)?
            .ThenBy(u => u.Email)?
            .ToList() ?? ListUtils.Empty<User>();
    }

    public async Task RemoveRoom(Guid roomId, string userId)
    {
        var request = new RemoveRoomRequest
        {
            RoomId = roomId,
            UserId = userId
        };

        await _validationService.Validate(roomId, userId, ValidatorType.RoomWrite, () => _roomManagementService.RemoveRoom(request));
    }

    public async Task ShareRoom(Guid roomId, string userId, string callerId)
    {
        var request = new ShareRoomToUserRequest
        {
            RoomId = roomId,
            CallerUserId = callerId,
            SubjectUserId = userId
        };

        await _validationService.Validate(roomId, callerId, ValidatorType.RoomWrite, () => _roomSharingService.ShareRoomToUser(request));
    }

    public async Task UnshareRoom(Guid roomId, string userId, string callerId)
    {
        var request = new UnshareRoomFromUserRequest
        {
            RoomId = roomId,
            CallerUserId = callerId,
            SubjectUserId = userId
        };

        await _validationService.Validate(roomId, callerId, ValidatorType.RoomWrite, () => _roomSharingService.UnshareRoomFromUser(request));
    }
}
