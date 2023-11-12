using iHome.Microservices.Schedules.Contract.Models;
using iHome.Model;

namespace iHome.Infrastructure.Mappers;

internal static class ScheduleModelExtensions
{
    internal static ScheduleDto ToDto(this ScheduleModel schedule)
    {
        return new ScheduleDto
        {
            Id = schedule.Id,
            Name = schedule.Name,
            Hour = schedule.Hour,
            Minute = schedule.Minute,
            Runned = schedule.Runned,
            UserId = schedule.UserId
        };
    }
}
