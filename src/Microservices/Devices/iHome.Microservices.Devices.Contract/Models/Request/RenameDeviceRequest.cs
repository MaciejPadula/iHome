using System;

namespace iHome.Microservices.Devices.Contract.Models.Request
{
    public class RenameDeviceRequest
    {
        public Guid DeviceId { get; set; }
        public string NewName { get; set; }
        public string UserId { get; set; }
    }

}
