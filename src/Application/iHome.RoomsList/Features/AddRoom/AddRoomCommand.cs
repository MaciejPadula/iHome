using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.RoomsList.Features.AddRoom;

public class AddRoomCommand : ICommand
{
    public required string RoomName { get; set; }
    public required string UserId { get; set; }
}
