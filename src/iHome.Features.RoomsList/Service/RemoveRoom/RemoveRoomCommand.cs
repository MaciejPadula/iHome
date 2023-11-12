using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.Features.RoomsList.Service.RemoveRoom;

internal class RemoveRoomCommand : ICommand
{
    public Guid RoomId { get; set; }
    public string UserId { get; set; }
}
