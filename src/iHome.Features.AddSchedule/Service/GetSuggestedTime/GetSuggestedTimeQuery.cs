using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.Features.AddSchedule.Service.GetSuggestedTime;

internal class GetSuggestedTimeQuery : IQuery<TimeModel>
{
    public required string Name { get; set; }
    public TimeModel Result { get; set; }
}
