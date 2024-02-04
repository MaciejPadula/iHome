using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.Users.Features.GetUsersByIds;

public class GetUsersByIdsQuery : IQuery<Dictionary<string, UserDto>>
{
    public required IEnumerable<string> Ids { get; set; }
    public Dictionary<string, UserDto> Result { get; set; } = default!;
}
