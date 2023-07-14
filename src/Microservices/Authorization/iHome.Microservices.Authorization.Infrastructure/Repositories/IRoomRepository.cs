namespace iHome.Microservices.Authorization.Infrastructure.Repositories;

public interface IRoomRepository
{
    Task<bool> UserHasWriteAccess(Guid roomId, string userId);
    Task<bool> UserHasReadAccess(Guid roomId, string userId);
}
