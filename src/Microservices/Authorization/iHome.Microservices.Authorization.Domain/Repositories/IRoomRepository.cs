namespace iHome.Microservices.Authorization.Domain.Repositories;

public interface IRoomRepository
{
    Task<bool> UserHasWriteAccess(Guid roomId, string userId);
    Task<bool> UserHasReadAccess(Guid roomId, string userId);
}
