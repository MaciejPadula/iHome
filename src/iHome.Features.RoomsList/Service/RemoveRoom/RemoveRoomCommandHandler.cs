using iHome.Core.Services.Validation;
using iHome.Model;
using iHome.Repository;
using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.Features.RoomsList.Service.RemoveRoom;
internal class RemoveRoomCommandHandler : IAsyncCommandHandler<RemoveRoomCommand>
{
    private readonly IValidationService _validationService;
    private readonly IRoomRepository _roomRepository;

    public RemoveRoomCommandHandler(IValidationService validationService, IRoomRepository roomRepository)
    {
        _validationService = validationService;
        _roomRepository = roomRepository;
    }

    public async Task HandleAsync(RemoveRoomCommand command)
    {
        await _validationService.Validate(
            command.RoomId,
            command.UserId,
            ValidatorType.RoomWrite,
            () => _roomRepository.Remove(command.RoomId));
    }
}
