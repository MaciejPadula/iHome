namespace iHome.Devices.Contract.Models.Requests;

public class ChangeDeviceRoomRequest
{
    public required Guid DeviceId { get; set; }
    public required Guid RoomId { get; set; }
}
