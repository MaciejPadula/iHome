using iHome.Repository;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.Features.RoomsList.Service.GetUserRooms;
internal class GetUserRoomsQueryHandler : IAsyncQueryHandler<GetUserRoomsQuery>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IUserRepository _userRepository;

    public GetUserRoomsQueryHandler(IRoomRepository roomRepository, IUserRepository userRepository)
    {
        _roomRepository = roomRepository;
        _userRepository = userRepository;
    }

    public async Task HandleAsync(GetUserRoomsQuery query)
    {
        var rooms = await _roomRepository.GetUserRooms(query.UserId);
        var userIds = await _userRepository.GetUsers(rooms.Select(r => r.UserId).Distinct());

        foreach (var room in rooms)
        {
            room.User = userIds.GetValueOrDefault(room.UserId);
        }

        query.Result = rooms;
    }
}
