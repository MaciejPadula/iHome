using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.Features.SchedulesList.Service.UpdateScheduleTime;

internal class UpdateScheduleTimeCommand : ICommand
{
    public Guid Id { get; set; }
    public int Day { get; set; }
    public string Time { get; set; } = default!;
    public string UserId { get; set; } = default!;
}
