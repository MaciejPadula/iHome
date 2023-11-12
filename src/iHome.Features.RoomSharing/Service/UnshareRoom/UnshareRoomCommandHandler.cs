using iHome.Core.Services.Validation;
using iHome.Model;
using iHome.Repository;
using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.Features.RoomSharing.Service.UnshareRoom;

internal class UnshareRoomCommandHandler : IAsyncCommandHandler<UnshareRoomCommand>
{
    private readonly IValidationService _validationService;
    private readonly IRoomSharesRepository _roomSharesRepository;

    public UnshareRoomCommandHandler(IValidationService validationService, IRoomSharesRepository roomSharesRepository)
    {
        _validationService = validationService;
        _roomSharesRepository = roomSharesRepository;
    }

    public async Task HandleAsync(UnshareRoomCommand command)
    {
        await _validationService.Validate(
            command.RoomId,
            command.CallerId,
            ValidatorType.RoomWrite,
            () => _roomSharesRepository.Remove(command.RoomId, command.UserId));
    }
}
