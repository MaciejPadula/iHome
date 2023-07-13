using iHome.Microservices.Authorization.Contract.Models.Request;
using iHome.Microservices.Authorization.Infrastructure.Repositories;

namespace iHome.Microservices.Authorization.Managers;

public interface IScheduleManager
{
    Task<bool> CanAccess(ScheduleAuthRequest request);
}

public class ScheduleManager : IScheduleManager
{
    private readonly IScheduleRepostitory _scheduleRepostitory;

    public ScheduleManager(IScheduleRepostitory scheduleRepostitory)
    {
        _scheduleRepostitory = scheduleRepostitory;
    }

    public Task<bool> CanAccess(ScheduleAuthRequest request) =>
        _scheduleRepostitory.UserHasAccess(request.ScheduleId, request.UserId);
}
