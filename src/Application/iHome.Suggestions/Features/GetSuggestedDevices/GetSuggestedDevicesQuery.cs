using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.Suggestions.Features.GetSuggestedDevices;

internal class GetSuggestedDevicesQuery : IQuery<List<Guid>>
{
    public required string Name { get; set; }
    public required TimeModel Time { get; set; }
    public required string UserId { get; set; }
    public List<Guid> Result { get; set; } = default!;
}
