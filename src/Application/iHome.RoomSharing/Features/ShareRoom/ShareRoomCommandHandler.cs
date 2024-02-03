using iHome.Core.Services.Validation;
using iHome.Microservices.RoomsManagement.Contract;
using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.RoomSharing.Features.ShareRoom;

internal class ShareRoomCommandHandler : IAsyncCommandHandler<ShareRoomCommand>
{
    private readonly IValidationService _validationService;
    private readonly IRoomSharingService _roomSharingService;

    public ShareRoomCommandHandler(IValidationService validationService, IRoomSharingService roomSharingService)
    {
        _validationService = validationService;
        _roomSharingService = roomSharingService;
    }

    public async Task HandleAsync(ShareRoomCommand command)
    {
        await _validationService.Validate(
            command.RoomId,
            command.CallerId,
            ValidatorType.RoomWrite,
            () => _roomSharingService.ShareRoomToUser(new()
            {
                RoomId = command.RoomId,
                SubjectUserId = command.UserId
            }));
    }
}
