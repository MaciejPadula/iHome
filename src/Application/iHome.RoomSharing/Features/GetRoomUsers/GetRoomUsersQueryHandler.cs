using iHome.Core.Services.Validation;
using iHome.Microservices.RoomsManagement.Contract;
using iHome.Microservices.UsersApi.Contract;
using iHome.Model;
using iHome.RoomSharing.Features.Shared.Mappers;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.RoomSharing.Features.GetRoomUsers;

internal class GetRoomUsersQueryHandler : IAsyncQueryHandler<GetRoomUsersQuery>
{
    private readonly IValidationService _validationService;
    private readonly IRoomSharingService _roomSharingService;
    private readonly IUserManagementService _userManagementService;

    public GetRoomUsersQueryHandler(IValidationService validationService, IRoomSharingService roomSharingService, IUserManagementService userManagementService)
    {
        _validationService = validationService;
        _roomSharingService = roomSharingService;
        _userManagementService = userManagementService;
    }

    public async Task HandleAsync(GetRoomUsersQuery query)
    {
        await _validationService.Validate(
            query.RoomId,
            query.UserId,
            ValidatorType.RoomWrite,
            async () =>
            {
                var result = await _roomSharingService.GetRoomUserIds(new()
                {
                    RoomId = query.RoomId,
                    UserId = query.UserId
                });
                var users = await GetUsers(result.UsersIds);
                query.Result = users.Values;
            });
    }

    private async Task<Dictionary<string, UserDto>> GetUsers(IEnumerable<string> userIds)
    {
        var response = await _userManagementService.GetUsersByIds(new()
        {
            Ids = userIds
        });

        return response?
            .Users?
            .Select(u => new KeyValuePair<string, UserDto>(u.Key, u.Value.ToDto()))?
            .ToDictionary(u => u.Key, u => u.Value) ?? [];
    }
}
