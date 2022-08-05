using iHome.Models.Database;
using iHome.Models.DataModels;

namespace iHome.Logic.Utils
{
    public class DataModelsConversionUtils
    {
        public static Device DeviceFromTDevice(TDevice tDevice)
        {
            return new Device()
            {
                deviceId = tDevice.deviceId,
                deviceName = tDevice.deviceName,
                deviceType = tDevice.deviceType,
                deviceData = tDevice.deviceData,
            };
        }
        public static List<Device> ListOfDevicesFromListOfTDevices(List<TDevice> tDevicesList)
        {
            var list = new List<Device>();
            tDevicesList.ForEach(device =>
            {
                list.Add(DeviceFromTDevice(device));
            });
            return list;
        }
    }
}
