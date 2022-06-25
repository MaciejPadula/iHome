using iHome.Models.Database;

namespace iHome.Models.DataModels
{
    public class DataModelsConversionUtils
    {
        public static Room RoomFromTRoom(TRoom tRoom)
        {
            return new Room() {
                roomId = tRoom.roomId,
                roomName = tRoom.roomName,
                roomDescription = tRoom.roomDescription,
                roomImage = tRoom.roomImage,
                devices = ListOfDevicesFromListOfTDevices(tRoom.devices)
            };
        }
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
