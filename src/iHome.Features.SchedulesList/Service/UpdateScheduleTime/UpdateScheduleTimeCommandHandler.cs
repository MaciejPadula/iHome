using iHome.Core.Services.Validation;
using iHome.Model;
using iHome.Repository;
using iHome.Shared.Logic;
using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.Features.SchedulesList.Service.UpdateScheduleTime;

internal class UpdateScheduleTimeCommandHandler : IAsyncCommandHandler<UpdateScheduleTimeCommand>
{
    private readonly IValidationService _validationService;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly ITimeModelParser _timeModelParser;

    public UpdateScheduleTimeCommandHandler(IValidationService validationService, IScheduleRepository scheduleRepository, ITimeModelParser timeModelParser)
    {
        _validationService = validationService;
        _scheduleRepository = scheduleRepository;
        _timeModelParser = timeModelParser;
    }

    public async Task HandleAsync(UpdateScheduleTimeCommand command)
    {
        var time = _timeModelParser.Parse(command.Time);

        await _validationService.Validate(
            command.Id,
            command.UserId,
            ValidatorType.Schedule,
            () => _scheduleRepository.UpdateScheduleTime(command.Id, time, command.Day));
    }
}
