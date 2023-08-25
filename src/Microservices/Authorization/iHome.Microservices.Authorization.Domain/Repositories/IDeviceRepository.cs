namespace iHome.Microservices.Authorization.Domain.Repositories;

public interface IDeviceRepository
{
    Task<bool> UserHasWriteAccess(Guid deviceId, string userId);
    Task<bool> UserHasReadAccess(Guid deviceId, string userId);
}
