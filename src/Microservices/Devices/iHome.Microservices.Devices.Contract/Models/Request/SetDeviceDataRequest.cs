using System;

namespace iHome.Microservices.Devices.Contract.Models.Request
{
    public class SetDeviceDataRequest
    {
        public Guid DeviceId { get; set; }
        public string Data { get; set; }
        public string UserId { get; set; }
    }
}
