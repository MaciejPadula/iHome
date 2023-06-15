using System;

namespace iHome.Microservices.Devices.Contract.Models.Request
{
    public class RemoveDeviceRequest
    {
        public Guid DeviceId { get; set; }
    }
}
