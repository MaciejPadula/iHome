using System;

namespace iHome.Microservices.Devices.Contract.Models.Request
{
    public class AddDeviceRequest
    {
        public string Name { get; set; }
        public string MacAddress { get; set; }
        public DeviceType Type { get; set; }
        public Guid RoomId { get; set; }
        public string UserId { get; set; }
    }
}
