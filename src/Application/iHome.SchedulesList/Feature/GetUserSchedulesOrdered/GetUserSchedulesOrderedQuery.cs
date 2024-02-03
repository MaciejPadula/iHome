using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.SchedulesList.Feature.GetUserSchedulesOrdered;

internal class GetUserSchedulesOrderedQuery : IQuery<List<ScheduleDto>>
{
    public required string UserId { get; set; }
    public List<ScheduleDto> Result { get; set; } = default!;
}
