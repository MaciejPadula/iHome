using iHome.Core.Services.Validation;
using iHome.Microservices.Schedules.Contract;
using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.SchedulesList.Feature.RemoveSchedule;

internal class RemoveScheduleCommandHandler : IAsyncCommandHandler<RemoveScheduleCommand>
{
    private readonly IValidationService _validationService;
    private readonly IScheduleManagementService _scheduleManagementService;

    public RemoveScheduleCommandHandler(IValidationService validationService, IScheduleManagementService scheduleManagementService)
    {
        _validationService = validationService;
        _scheduleManagementService = scheduleManagementService;
    }

    public async Task HandleAsync(RemoveScheduleCommand command)
    {
        await _validationService.Validate(
            command.Id,
            command.UserId,
            ValidatorType.Schedule,
            () => _scheduleManagementService.RemoveSchedule(new() { ScheduleId = command.Id }));
    }
}
