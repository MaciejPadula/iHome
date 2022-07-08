namespace iHome.Models.Account.Rooms.Requests
{
    public class NewDeviceToConfigureRequest
    {
        public string? deviceId { get; set; }
        public int deviceType { get; set; }
    }
}
