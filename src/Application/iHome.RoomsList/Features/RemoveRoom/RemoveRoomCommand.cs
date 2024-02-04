using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.RoomsList.Features.RemoveRoom;

public class RemoveRoomCommand : ICommand
{
    public required Guid RoomId { get; set; }
    public string UserId { get; set; } = default!;
}
