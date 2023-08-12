using iHome.Core.Logic.RoomDtoList;
using iHome.Core.Models;
using iHome.Core.Services.Validation;
using iHome.Microservices.RoomsManagement.Contract;
using iHome.Microservices.RoomsManagement.Contract.Models.Request;
using iHome.Microservices.UsersApi.Contract;

namespace iHome.Core.Services;

public interface ISharingService
{
    Task ShareRoom(Guid roomId, string userId, string callerId);
    Task UnshareRoom(Guid roomId, string userId, string callerId);
}

public class SharingService : ISharingService
{
    private readonly IRoomSharingService _roomSharingService;
    private readonly IValidationService _validationService;

    public SharingService(IRoomSharingService roomSharingService, IValidationService validationService)
    {
        _roomSharingService = roomSharingService;
        _validationService = validationService;
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
