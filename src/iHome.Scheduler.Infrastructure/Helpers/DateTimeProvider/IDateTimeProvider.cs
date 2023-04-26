﻿namespace iHome.Scheduler.Infrastructure.Helpers.DateTimeProvider;

public interface IDateTimeProvider
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
}