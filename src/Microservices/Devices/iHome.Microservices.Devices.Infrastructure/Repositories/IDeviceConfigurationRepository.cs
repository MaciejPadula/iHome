using iHome.Microservices.Devices.Contract.Models;

namespace iHome.Microservices.Devices.Infrastructure.Repositories
{
    public interface IDeviceConfigurationRepository
    {
        Task<IEnumerable<DeviceToConfigure>> GetDeviceToConfigure(string ipAddress);
        Task RemoveDeviceToConfigure(string ipAddress, Guid id);
    }
}
