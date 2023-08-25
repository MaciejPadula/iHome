namespace iHome.Microservices.Authorization.Domain.Repositories;

public interface IScheduleRepostitory
{
    Task<bool> UserHasAccess(Guid scheduleId, string userId);
}
