using System;

namespace iHome.Microservices.Devices.Contract.Models
{
    public class DeviceModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DeviceType Type { get; set; }
        public Guid RoomId { get; set; }
        public string MacAddress { get; set; }
        public string Data { get; set; }
    }
}