using iHome.Repository;
using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.Features.RoomsList.Service.AddRoom;

internal class AddRoomCommandHandler : IAsyncCommandHandler<AddRoomCommand>
{
    private readonly IRoomRepository _roomRepository;

    public AddRoomCommandHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task HandleAsync(AddRoomCommand command)
    {
        await _roomRepository.Add(command.RoomName, command.UserId);
    }
}
