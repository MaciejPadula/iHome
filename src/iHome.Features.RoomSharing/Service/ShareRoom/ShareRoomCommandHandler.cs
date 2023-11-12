using iHome.Core.Services.Validation;
using iHome.Model;
using iHome.Repository;
using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.Features.RoomSharing.Service.ShareRoom;

internal class ShareRoomCommandHandler : IAsyncCommandHandler<ShareRoomCommand>
{
    private readonly IValidationService _validationService;
    private readonly IRoomSharesRepository _roomSharesRepository;

    public ShareRoomCommandHandler(IValidationService validationService, IRoomSharesRepository roomSharesRepository)
    {
        _validationService = validationService;
        _roomSharesRepository = roomSharesRepository;
    }

    public async Task HandleAsync(ShareRoomCommand command)
    {
        await _validationService.Validate(
            command.RoomId,
            command.CallerId,
            ValidatorType.RoomWrite,
            () => _roomSharesRepository.Add(command.RoomId, command.UserId));
    }
}
