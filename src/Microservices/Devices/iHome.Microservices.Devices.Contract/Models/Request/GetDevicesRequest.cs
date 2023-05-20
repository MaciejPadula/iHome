using System;

namespace iHome.Microservices.Devices.Contract.Models.Request
{
    public class GetDevicesRequest
    {
        public Guid RoomId { get; set; }
        public string UserId { get; set; }
    }
}
