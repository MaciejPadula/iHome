namespace iHome.Microservices.Authorization.Infrastructure.Repositories;

public interface IScheduleRepostitory
{
    Task<bool> UserHasAccess(Guid scheduleId, string userId);
}
