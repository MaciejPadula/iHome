using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.Features.AddSchedule.Service.AddSchedule;

public class AddScheduleCommand : ICommand
{
    public string Name { get; set; } = default!;
    public int Day { get; set; }
    public string Time { get; set; } = default!;
    public string UserId { get; set; } = default!;
}
