using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.Suggestions.Features.GetSuggestedTime;

internal class GetSuggestedTimeQuery : IQuery<TimeModel>
{
    public required string Name { get; set; }
    public TimeModel Result { get; set; }
}
