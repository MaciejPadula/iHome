using iHome.Microservices.Authorization.Contract.Models.Request;
using iHome.Microservices.Authorization.Domain.Repositories;

namespace iHome.Microservices.Authorization.Managers;

public interface IDeviceManager
{
    Task<bool> CanRead(DeviceAuthRequest request);
    Task<bool> CanWrite(DeviceAuthRequest request);
}

public class DeviceManager : IDeviceManager
{
    private readonly IDeviceRepository _deviceRepository;

    public DeviceManager(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public Task<bool> CanRead(DeviceAuthRequest request) =>
        _deviceRepository.UserHasReadAccess(request.DeviceId, request.UserId);

    public Task<bool> CanWrite(DeviceAuthRequest request) =>
        _deviceRepository.UserHasWriteAccess(request.DeviceId, request.UserId);
}
