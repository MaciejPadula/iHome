using iHome.Microservices.UsersApi.Contract.Models;
using iHome.Microservices.UsersApi.Contract;
using Web.Infrastructure.Cqrs.Mediator.Query;
using iHome.Model;
using iHome.RoomSharing.Features.Shared.Mappers;

namespace iHome.Users.Features.GetUsers;

internal class GetUsersQueryHandler : IAsyncQueryHandler<GetUsersQuery>
{
    private readonly IUserManagementService _userManagementService;

    public GetUsersQueryHandler(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task HandleAsync(GetUsersQuery query)
    {
        var searchPhrase = query.SearchPhrase;

        if (searchPhrase.Length < 3)
        {
            query.Result = [];
            return;
        }

        var searchWildcard = $"*{searchPhrase}*";

        var filter = new UserFilter
        {
            Name = searchWildcard,
            Email = searchWildcard
        };

        var users = await _userManagementService.GetUsers(new() { Filter = filter });

        query.Result = users?.Users?
            .Select(x => x.ToDto())?
            .ToList() ?? [];
    }
}
