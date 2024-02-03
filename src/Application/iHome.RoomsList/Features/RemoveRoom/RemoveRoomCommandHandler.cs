using iHome.Core.Services.Validation;
using iHome.Microservices.RoomsManagement.Contract;
using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.RoomsList.Features.RemoveRoom;

internal class RemoveRoomCommandHandler : IAsyncCommandHandler<RemoveRoomCommand>
{
    private readonly IValidationService _validationService;
    private readonly IRoomManagementService _roomManagementService;

    public RemoveRoomCommandHandler(IValidationService validationService, IRoomManagementService roomManagementService)
    {
        _validationService = validationService;
        _roomManagementService = roomManagementService;
    }

    public async Task HandleAsync(RemoveRoomCommand command)
    {
        await _validationService.Validate(
            command.RoomId,
            command.UserId,
            ValidatorType.RoomWrite,
            () => _roomManagementService.RemoveRoom(new()
            {
                RoomId = command.RoomId,
                UserId = command.UserId
            }));
    }
}
