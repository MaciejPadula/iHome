using iHome.Core.Services.Validation;
using iHome.Microservices.Schedules.Contract;
using iHome.Model;
using iHome.Shared.Logic;
using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.SchedulesList.Feature.UpdateScheduleTime;

internal class UpdateScheduleTimeCommandHandler : IAsyncCommandHandler<UpdateScheduleTimeCommand>
{
    private readonly IValidationService _validationService;
    private readonly ITimeModelParser _timeModelParser;
    private readonly IScheduleManagementService _scheduleManagementService;

    public UpdateScheduleTimeCommandHandler(IValidationService validationService, ITimeModelParser timeModelParser, IScheduleManagementService scheduleManagementService)
    {
        _validationService = validationService;
        _timeModelParser = timeModelParser;
        _scheduleManagementService = scheduleManagementService;
    }

    public async Task HandleAsync(UpdateScheduleTimeCommand command)
    {
        var time = _timeModelParser.Parse(command.Time);

        await _validationService.Validate(
            command.Id,
            command.UserId,
            ValidatorType.Schedule,
            () => _scheduleManagementService.UpdateScheduleTime(new()
            {
                ScheduleId = command.Id,
                Hour = time.Hour,
                Minute = time.Minute,
                Day = command.Day
            }));
    }
}
