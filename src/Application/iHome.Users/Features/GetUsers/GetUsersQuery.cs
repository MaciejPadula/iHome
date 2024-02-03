using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.Users.Features.GetUsers;

internal class GetUsersQuery : IQuery<List<UserDto>>
{
    public required string SearchPhrase { get; set; }
    public List<UserDto> Result { get; set; } = default!;
}
