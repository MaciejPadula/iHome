using iHome.Devices.Contract.Models.Requests;

namespace iHome.Devices.Contract.Interfaces;

public interface IDeviceManipulator
{
    Guid AddDevice(AddDeviceRequest request);
    void SetDeviceData(SetDeviceDataRequest request);
    void RemoveDevice(RemoveDeviceRequest request);

    void ChangeDeviceName(ChangeDeviceNameRequest request);
    void ChangeDeviceRoom(ChangeDeviceRoomRequest request);
}
