using iHome.Core.Helpers;
using iHome.Core.Models;
using iHome.Core.Services.Validation;
using iHome.Microservices.RoomsManagement.Contract;
using iHome.Microservices.RoomsManagement.Contract.Models.Request;
using iHome.Microservices.UsersApi.Contract;
using iHome.Microservices.UsersApi.Contract.Models;

namespace iHome.Core.Services;

public interface IUserService
{
    Task<List<User>> GetUsers(string searchPhrase);
    Task<List<User>> GetRoomUsers(Guid roomId, string userId);
}

public class UserService : IUserService
{
    private readonly IUserManagementService _userManagementService;
    private readonly IRoomSharingService _roomSharingService;
    private readonly IValidationService _validationService;

    public UserService(IUserManagementService userManagementService,
        IRoomSharingService roomSharingService,
        IValidationService validationService)
    {
        _userManagementService = userManagementService;
        _roomSharingService = roomSharingService;
        _validationService = validationService;
    }

    public async Task<List<User>> GetUsers(string searchPhrase)
    {
        if (searchPhrase.Length < 3)
        {
            return Enumerable.Empty<User>().ToList();
        }

        var searchWildcard = $"*{searchPhrase}*";

        var filter = new UserFilter
        {
            Name = searchWildcard,
            Email = searchWildcard
        };

        var users = await _userManagementService.GetUsers(new() { Filter = filter });

        return users?.Users?.ToList() ?? Enumerable.Empty<User>().ToList();
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
}
