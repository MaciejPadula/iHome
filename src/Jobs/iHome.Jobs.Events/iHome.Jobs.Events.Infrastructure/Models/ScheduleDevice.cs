﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iHome.Jobs.Events.Infrastructure.Models;

[Table("SchedulesDevices", Schema = "maciejadmin")]
public class ScheduleDevice
{
    [Key]
    public Guid Id { get; set; } = new Guid();
    public required Guid ScheduleId { get; set; }
    public required Guid DeviceId { get; set; }
    public required string DeviceData { get; set; }

    public virtual Schedule? Schedule { get; set; }
}
