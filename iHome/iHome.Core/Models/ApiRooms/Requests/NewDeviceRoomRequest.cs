namespace iHome.Models.Requests
{
    public class NewDeviceRoomRequest
    {
        public string DeviceId { get; set; }
        public int RoomId { get; set; }

        public NewDeviceRoomRequest(string deviceId, int roomId)
        {
            DeviceId = deviceId;
            RoomId = roomId;
        }
    }
}
