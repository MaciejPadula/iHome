﻿using System;

namespace iHome.Microservices.Schedules.Contract.Models.Request
{
    public class GetDevicesInScheduleCountRequest
    {
        public Guid ScheduleId { get; set; }
        public string UserId { get; set; }
    }
}
