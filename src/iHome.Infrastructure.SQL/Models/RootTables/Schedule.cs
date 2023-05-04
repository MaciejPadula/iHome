using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iHome.Infrastructure.SQL.Models.ChildrenTables;
using iHome.Infrastructure.SQL.Models.ConnectionTables;

namespace iHome.Infrastructure.SQL.Models.RootTables;

[Table("Schedules", Schema = "maciejadmin")]
public class Schedule
{
    [Key]
    public Guid Id { get; set; } = new Guid();
    public required string Name { get; set; }
    public required string ActivationCron { get; set; }
    public DateTime Modified { get; set; } = DateTime.UtcNow;

    public required string UserId { get; set; }

    public virtual ICollection<ScheduleDevice> ScheduleDevices { get; set; } = new List<ScheduleDevice>();
    public virtual ICollection<ScheduleRunHistory> ScheduleRuns { get; set; } = new List<ScheduleRunHistory>();
}
