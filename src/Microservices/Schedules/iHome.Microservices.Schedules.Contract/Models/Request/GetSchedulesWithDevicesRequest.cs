using System;
using System.Collections.Generic;

namespace iHome.Microservices.Schedules.Contract.Models.Request
{
    public class GetSchedulesWithDevicesRequest
    {
        public IEnumerable<Guid> DeviceIds { get; set; }
    }
}
