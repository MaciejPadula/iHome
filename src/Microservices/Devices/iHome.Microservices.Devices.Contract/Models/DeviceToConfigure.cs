using System;

namespace iHome.Microservices.Devices.Contract.Models
{
    public class DeviceToConfigure
    {
        public Guid Id { get; set; }
        public string MacAddress { get; set; }
        public DeviceType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
