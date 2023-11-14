using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.Features.SchedulesList.Service.RemoveSchedule;

internal class RemoveScheduleCommand : ICommand
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = default!;
}
