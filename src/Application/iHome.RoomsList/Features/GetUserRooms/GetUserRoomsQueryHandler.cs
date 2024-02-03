using iHome.Microservices.RoomsManagement.Contract;
using iHome.Microservices.UsersApi.Contract;
using iHome.Model;
using iHome.RoomsList.Features.Shared.Mappers;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.RoomsList.Features.GetUserRooms;

internal class GetUserRoomsQueryHandler : IAsyncQueryHandler<GetUserRoomsQuery>
{
    private readonly IRoomManagementService _roomManagementService;
    private readonly IUserManagementService _userManagementService;

    public GetUserRoomsQueryHandler(IRoomManagementService roomManagementService, IUserManagementService userManagementService)
    {
        _roomManagementService = roomManagementService;
        _userManagementService = userManagementService;
    }

    public async Task HandleAsync(GetUserRoomsQuery query)
    {
        var rooms = (await _roomManagementService.GetRooms(new() { UserId = query.UserId }))
            .Rooms
            .Select(x => x.ToDto())
            .ToList();

        var users = await GetUsers(rooms.Select(r => r.UserId).Distinct());

        foreach (var room in rooms)
        {
            room.User = users.GetValueOrDefault(room.UserId);
        }

        query.Result = rooms;
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
