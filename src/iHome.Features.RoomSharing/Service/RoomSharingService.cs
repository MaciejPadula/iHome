using iHome.Features.RoomSharing.Service.GetRoomUsers;
using iHome.Features.RoomSharing.Service.ShareRoom;
using iHome.Features.RoomSharing.Service.UnshareRoom;
using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator;

namespace iHome.Features.RoomSharing.Service;

internal class RoomSharingService : IRoomSharingService
{
    private readonly IMediator _mediator;

    public RoomSharingService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IEnumerable<UserDto>> GetRoomUsers(Guid roomId, string userId)
    {
        var query = await _mediator.HandleQueryAsync(new GetRoomUsersQuery
        {
            RoomId = roomId,
            UserId = userId
        });

        return query.Result;
    }

    public async Task ShareRoom(Guid roomId, string userId, string callerId)
    {
        await _mediator.HandleCommandAsync(new ShareRoomCommand
        {
            RoomId = roomId,
            UserId = userId,
            CallerId = callerId
        });
    }

    public async Task UnshareRoom(Guid roomId, string userId, string callerId)
    {
        await _mediator.HandleCommandAsync(new UnshareRoomCommand
        {
            RoomId = roomId,
            UserId = userId,
            CallerId = callerId
        });
    }
}
