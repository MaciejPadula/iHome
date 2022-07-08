namespace iHome.Models.Database
{
    public class TDeviceToConfigure
    {
        public int id { get; set; }
        public string? deviceId { get; set; }
        public int deviceType { get; set; }
        public string? ipAddress { get; set; }
    }
}
