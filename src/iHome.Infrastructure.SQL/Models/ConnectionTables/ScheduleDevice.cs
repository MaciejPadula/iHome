using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iHome.Infrastructure.SQL.Models.RootTables;

namespace iHome.Infrastructure.SQL.Models.ConnectionTables;

[Table("SchedulesDevices", Schema = "maciejadmin")]
public class ScheduleDevice
{
    [Key]
    public Guid Id { get; set; } = new Guid();
    public required Guid ScheduleId { get; set; }
    public required Guid DeviceId { get; set; }
    public required string DeviceData { get; set; }

    public virtual Schedule? Schedule { get; set; }
    public virtual Device? Device { get; set; }
}
