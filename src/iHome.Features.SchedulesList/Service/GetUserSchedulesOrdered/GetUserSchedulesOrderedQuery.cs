using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.Features.SchedulesList.Service.GetUserSchedulesOrdered;

internal class GetUserSchedulesOrderedQuery : IQuery<List<ScheduleDto>>
{
    public string UserId { get; set; } = default!;
    public List<ScheduleDto> Result { get; set; } = default!;
}
