namespace iHome.Models.Account.Rooms.Requests
{
    public class NewDeviceToConfigureRequest
    {
        public string DeviceId { get; set; } = "";
        public int DeviceType { get; set; }
    }
}
