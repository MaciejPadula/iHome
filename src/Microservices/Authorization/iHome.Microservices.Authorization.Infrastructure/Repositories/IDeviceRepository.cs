namespace iHome.Microservices.Authorization.Infrastructure.Repositories;

public interface IDeviceRepository
{
    Task<bool> UserHasWriteAccess(Guid deviceId, string userId);
    Task<bool> UserHasReadAccess(Guid deviceId, string userId);
}
