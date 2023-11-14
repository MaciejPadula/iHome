using iHome.Model;
using iHome.Repository;
using iHome.Shared.Logic;
using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.Features.AddSchedule.Service.AddSchedule;

internal class AddScheduleCommandHandler : IAsyncCommandHandler<AddScheduleCommand>
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly ITimeModelParser _timeModelParser;

    public AddScheduleCommandHandler(IScheduleRepository scheduleRepository, ITimeModelParser timeModelParser)
    {
        _scheduleRepository = scheduleRepository;
        _timeModelParser = timeModelParser;
    }

    public async Task HandleAsync(AddScheduleCommand command)
    {
        var time = _timeModelParser.Parse(command.Time);

        await _scheduleRepository.Add(new ScheduleDto
        {
            Name = command.Name,
            UserId = command.UserId,
            Hour = time.Hour,
            Minute = time.Minute
        });
    }
}
