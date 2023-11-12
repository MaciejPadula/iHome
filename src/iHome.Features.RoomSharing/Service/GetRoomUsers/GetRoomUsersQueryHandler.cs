using iHome.Core.Services.Validation;
using iHome.Model;
using iHome.Repository;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.Features.RoomSharing.Service.GetRoomUsers;

internal class GetRoomUsersQueryHandler : IAsyncQueryHandler<GetRoomUsersQuery>
{
    private readonly IValidationService _validationService;
    private readonly IRoomSharesRepository _roomSharesRepository;
    private readonly IUserRepository _userRepository;

    public GetRoomUsersQueryHandler(IValidationService validationService, IRoomSharesRepository roomSharesRepository, IUserRepository userRepository)
    {
        _validationService = validationService;
        _roomSharesRepository = roomSharesRepository;
        _userRepository = userRepository;
    }

    public async Task HandleAsync(GetRoomUsersQuery query)
    {
        await _validationService.Validate(
            query.RoomId,
            query.UserId,
            ValidatorType.RoomWrite,
            async () =>
            {
                var usersIds = await _roomSharesRepository.GetRoomUsersIds(query.RoomId);
                var users = await _userRepository.GetUsers(usersIds);
                query.Result = users.Values;
            });
    }
}
