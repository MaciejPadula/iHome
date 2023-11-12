using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.Features.RoomSharing.Service.UnshareRoom;

internal class UnshareRoomCommand : ICommand
{
    public Guid RoomId { get; set; }
    public string UserId { get; set; }
    public string CallerId { get; set; }
}
