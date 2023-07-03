using System;
using System.Collections.Generic;

namespace iHome.Microservices.Schedules.Contract
{
    public class GetDevicesForSchedulingResponse
    {
        public IEnumerable<Guid> DeviceIds { get; set; }
    }
}