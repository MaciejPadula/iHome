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
                Id = tDevice.DeviceId,
                Name = tDevice.DeviceName,
                Type = tDevice.DeviceType,
                Data = tDevice.DeviceData,
                RoomId = tDevice.RoomId,
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
