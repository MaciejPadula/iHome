namespace iHome.Models.Requests
{
    public class NewDeviceRoomRequest
    {
        public string DeviceId { get; set; }
        public Guid RoomId { get; set; }

        public NewDeviceRoomRequest(string deviceId, Guid roomId)
        {
            DeviceId = deviceId;
            RoomId = roomId;
        }
    }
}
