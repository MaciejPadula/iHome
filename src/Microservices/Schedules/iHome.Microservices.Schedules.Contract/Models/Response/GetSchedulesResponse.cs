using System.Collections.Generic;

namespace iHome.Microservices.Schedules.Contract.Models.Response
{
    public class GetSchedulesResponse
    {
        public IEnumerable<ScheduleModel> Schedules { get; set; }
    }

}
