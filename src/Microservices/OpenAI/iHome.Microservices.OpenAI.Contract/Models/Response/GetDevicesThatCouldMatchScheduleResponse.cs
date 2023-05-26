using System;
using System.Collections.Generic;

namespace iHome.Microservices.OpenAI.Contract.Models.Response
{
    public class GetDevicesThatCouldMatchScheduleResponse
    {
        public IEnumerable<Guid> DevicesIds { get; set; }
    }
}
