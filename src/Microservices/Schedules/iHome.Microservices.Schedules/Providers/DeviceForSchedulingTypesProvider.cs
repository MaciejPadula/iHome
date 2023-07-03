using iHome.Microservices.Devices.Contract.Models;

namespace iHome.Microservices.Schedules.Providers
{
    public interface IDeviceForSchedulingTypesProvider
    {
        IEnumerable<DeviceType> Provide();
    }

    public class DeviceForSchedulingTypesProvider : IDeviceForSchedulingTypesProvider
    {
        private readonly List<DeviceType> _devicesForScheduling = new()
        {
            DeviceType.RGBLamp,
            DeviceType.RobotVaccumCleaner
        };

        public IEnumerable<DeviceType> Provide()
        {
            return _devicesForScheduling;
        }
    }
}
