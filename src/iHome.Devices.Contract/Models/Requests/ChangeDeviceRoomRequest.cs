namespace iHome.Devices.Contract.Models.Requests;

public class ChangeDeviceRoomRequest
{
    public Guid DeviceId { get; set; }
    public Guid RoomId { get; set; }
}
