using iHome.Microservices.Devices.Contract.Models;

namespace iHome.Microservices.Schedules.Infrastructure.Repositories
{
    public interface IDeviceRepository
    {
        Task<IEnumerable<Guid>> GetDevicesForUserByTypes(string userId, IEnumerable<DeviceType> deviceTypes);
    }
}
