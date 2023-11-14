using iHome.Core.Services.Validation;
using iHome.Model;
using iHome.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.Features.SchedulesList.Service.RemoveSchedule;

internal class RemoveScheduleCommandHandler : IAsyncCommandHandler<RemoveScheduleCommand>
{
    private readonly IValidationService _validationService;
    private readonly IScheduleRepository _scheduleRepository;

    public RemoveScheduleCommandHandler(IValidationService validationService, IScheduleRepository scheduleRepository)
    {
        _validationService = validationService;
        _scheduleRepository = scheduleRepository;
    }

    public async Task HandleAsync(RemoveScheduleCommand command)
    {
        await _validationService.Validate(
            command.Id,
            command.UserId,
            ValidatorType.Schedule,
            () => _scheduleRepository.Remove(command.Id));
    }
}
