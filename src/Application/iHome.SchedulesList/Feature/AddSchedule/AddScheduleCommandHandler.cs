using iHome.Microservices.Schedules.Contract;
using iHome.Shared.Logic;
using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.SchedulesList.Feature.AddSchedule;

internal class AddScheduleCommandHandler : IAsyncCommandHandler<AddScheduleCommand>
{
    private readonly IScheduleManagementService _scheduleManagementService;
    private readonly ITimeModelParser _timeModelParser;

    public AddScheduleCommandHandler(ITimeModelParser timeModelParser, IScheduleManagementService scheduleManagementService)
    {
        _timeModelParser = timeModelParser;
        _scheduleManagementService = scheduleManagementService;
    }

    public async Task HandleAsync(AddScheduleCommand command)
    {
        var time = _timeModelParser.Parse(command.Time);

        await _scheduleManagementService.AddSchedule(new()
        {
            ScheduleName = command.Name,
            UserId = command.UserId,
            Hour = time.Hour,
            Minute = time.Minute,
            Day = command.Day
        });
    }
}
