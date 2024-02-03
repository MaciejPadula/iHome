using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.SchedulesList.Feature.AddSchedule;

public class AddScheduleCommand : ICommand
{
    public required string Name { get; set; }
    public required int Day { get; set; }
    public required string Time { get; set; }
    public required string UserId { get; set; }
}
