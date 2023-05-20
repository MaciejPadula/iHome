using System;

namespace iHome.Microservices.Devices.Contract.Models.Request
{
    public class GetDeviceDataRequest
    {
        public Guid DeviceId { get; set; }
        public string UserId { get; set; }
    }
}
