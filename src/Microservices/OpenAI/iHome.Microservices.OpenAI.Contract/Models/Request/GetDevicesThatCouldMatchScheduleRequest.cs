using System.Collections.Generic;

namespace iHome.Microservices.OpenAI.Contract.Models.Request
{
    public class GetDevicesThatCouldMatchScheduleRequest
    {
        public string ScheduleName { get; set; }
        public string ScheduleTime { get; set; }
        public IEnumerable<DeviceDetails> Devices { get; set; }
    }
}
