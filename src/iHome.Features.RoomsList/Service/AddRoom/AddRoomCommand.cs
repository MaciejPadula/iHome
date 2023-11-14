using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.Features.RoomsList.Service.AddRoom;

internal class AddRoomCommand : ICommand
{
    public string RoomName { get; set; } = default!;
    public string UserId { get; set; } = default!;
}
