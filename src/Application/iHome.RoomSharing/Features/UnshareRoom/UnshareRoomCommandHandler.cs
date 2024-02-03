using iHome.Core.Services.Validation;
using iHome.Microservices.RoomsManagement.Contract;
using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.RoomSharing.Features.UnshareRoom;

internal class UnshareRoomCommandHandler : IAsyncCommandHandler<UnshareRoomCommand>
{
    private readonly IValidationService _validationService;
    private readonly IRoomSharingService _roomSharingService;

    public UnshareRoomCommandHandler(IValidationService validationService, IRoomSharingService roomSharingService)
    {
        _validationService = validationService;
        _roomSharingService = roomSharingService;
    }

    public async Task HandleAsync(UnshareRoomCommand command)
    {
        await _validationService.Validate(
            command.RoomId,
            command.CallerId,
            ValidatorType.RoomWrite,
            () => _roomSharingService.UnshareRoomFromUser(new()
            {
                RoomId = command.RoomId,
                SubjectUserId = command.UserId
            }));
    }
}
