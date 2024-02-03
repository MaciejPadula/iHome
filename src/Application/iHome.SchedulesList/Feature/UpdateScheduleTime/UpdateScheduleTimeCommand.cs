using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.SchedulesList.Feature.UpdateScheduleTime;

internal class UpdateScheduleTimeCommand : ICommand
{
    public required Guid Id { get; set; }
    public required int Day { get; set; }
    public required string Time { get; set; }
    public required string UserId { get; set; }
}
