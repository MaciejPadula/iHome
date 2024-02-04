using iHome.Microservices.UsersApi.Contract;
using iHome.Model;
using iHome.RoomSharing.Features.Shared.Mappers;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.Users.Features.GetUsersByIds;

internal class GetUsersByIdsQueryHandler : IAsyncQueryHandler<GetUsersByIdsQuery>
{
    private readonly IUserManagementService _userManagementService;

    public GetUsersByIdsQueryHandler(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task HandleAsync(GetUsersByIdsQuery query)
    {
        var response = await _userManagementService.GetUsersByIds(new()
        {
            Ids = query.Ids
        });

        query.Result = response?
            .Users?
            .Select(u => new KeyValuePair<string, UserDto>(u.Key, u.Value.ToDto()))?
            .ToDictionary(u => u.Key, u => u.Value) ?? [];
    }
}
