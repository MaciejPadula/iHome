﻿using Web.Infrastructure.Cqrs.Mediator.Command;

namespace iHome.SchedulesList.Feature.RemoveSchedule;

public class RemoveScheduleCommand : ICommand
{
    public required Guid Id { get; set; }
    public string UserId { get; set; } = default!;
}
