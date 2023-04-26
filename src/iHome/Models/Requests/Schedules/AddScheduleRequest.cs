﻿namespace iHome.Models.Requests.Schedules;

public class AddScheduleRequest
{
    public required string ScheduleName { get; set; }
    public required int Day { get; set; }
    public required int Hour { get; set; }
    public required int Minute { get; set; }
}