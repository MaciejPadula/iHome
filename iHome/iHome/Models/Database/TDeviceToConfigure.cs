namespace iHome.Models.Database
{
    public class TDeviceToConfigure
    {
        public int Id { get; set; }
        public string? DeviceId { get; set; }
        public int DeviceType { get; set; }
        public string? IpAddress { get; set; }
    }
}
