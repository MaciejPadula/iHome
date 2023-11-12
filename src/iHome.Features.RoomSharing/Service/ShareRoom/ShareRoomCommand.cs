using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.Features.RoomSharing.Service.ShareRoom;

internal class ShareRoomCommand : ICommand
{
    public Guid RoomId { get; set; }
    public string UserId { get; set; }
    public string CallerId { get; set; }
}
