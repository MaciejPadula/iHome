namespace iHome.Devices.Contract.Models.Requests;

public class GetDevicesRequest
{
    public required Guid RoomId { get; set; }
}
