using System;

namespace iHome.Microservices.Devices.Contract.Models.Request
{
    public class RemoveDeviceToConfigureRequest
    {
        public string Address { get; set; }
        public Guid DeviceToConfigureId { get; set; }
    }
}
