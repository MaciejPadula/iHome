using iHome.Microservices.RoomsManagement.Contract;
using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.RoomsList.Features.AddRoom;

internal class AddRoomCommandHandler : IAsyncCommandHandler<AddRoomCommand>
{
    private readonly IRoomManagementService _roomManagementService;

    public AddRoomCommandHandler(IRoomManagementService roomManagementService)
    {
        _roomManagementService = roomManagementService;
    }

    public async Task HandleAsync(AddRoomCommand command)
    {
        await _roomManagementService.AddRoom(new()
        {
            RoomName = command.RoomName,
            UserId = command.UserId
        });
    }
}
