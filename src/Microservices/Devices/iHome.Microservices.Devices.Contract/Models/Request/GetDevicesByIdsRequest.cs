using System;
using System.Collections.Generic;

namespace iHome.Microservices.Devices.Contract.Models.Request
{
    public class GetDevicesByIdsRequest
    {
        public IEnumerable<Guid> DeviceIds { get; set; }
        public string UserId { get; set; }
    }
}