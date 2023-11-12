using iHome.Features.RoomsList.Service.AddRoom;
using iHome.Features.RoomsList.Service.GetUserRooms;
using iHome.Features.RoomsList.Service.RemoveRoom;
using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator;

namespace iHome.Features.RoomsList.Service;

internal class RoomsListService : IRoomsListService
{
    private readonly IMediator _mediator;

    public RoomsListService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task AddRoom(string name, string userId)
    {
        await _mediator.HandleCommandAsync(new AddRoomCommand
        {
            RoomName = name,
            UserId = userId
        });
    }

    public async Task<IEnumerable<RoomDto>> GetUserRooms(string userId)
    {
        var query = await _mediator.HandleQueryAsync(new GetUserRoomsQuery
        {
            UserId = userId
        });

        return query.Result;
    }

    public async Task RemoveRoom(Guid id, string userId)
    {
        await _mediator.HandleCommandAsync(new RemoveRoomCommand
        {
            RoomId = id,
            UserId = userId
        });
    }
}
