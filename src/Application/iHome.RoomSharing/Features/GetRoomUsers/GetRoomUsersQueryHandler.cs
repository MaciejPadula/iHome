using iHome.Core.Services.Validation;
using iHome.Microservices.RoomsManagement.Contract;
using iHome.Microservices.UsersApi.Contract;
using iHome.Model;
using iHome.RoomSharing.Features.Shared.Mappers;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.RoomSharing.Features.GetRoomUsers;

internal class GetRoomUsersQueryHandler : IAsyncQueryHandler<GetRoomUsersQuery>
{
    private readonly IValidationService _validationService;
    private readonly IRoomSharingService _roomSharingService;

    public GetRoomUsersQueryHandler(IValidationService validationService, IRoomSharingService roomSharingService)
    {
        _validationService = validationService;
        _roomSharingService = roomSharingService;
    }

    public async Task HandleAsync(GetRoomUsersQuery query)
    {
        await _validationService.Validate(
            query.RoomId,
            query.UserId,
            ValidatorType.RoomWrite,
            async () =>
            {
                var result = await _roomSharingService.GetRoomUserIds(new()
                {
                    RoomId = query.RoomId,
                    UserId = query.UserId
                });
                query.Result = result.UsersIds.ToList();
            });
    }

    
}
