using System.Collections.Generic;

namespace iHome.Microservices.Schedules.Contract.Models.Response
{
    public class GetScheduleDevicesResponse
    {
        public IEnumerable<ScheduleDeviceModel> ScheduleDevices { get; set; }
    }
}
