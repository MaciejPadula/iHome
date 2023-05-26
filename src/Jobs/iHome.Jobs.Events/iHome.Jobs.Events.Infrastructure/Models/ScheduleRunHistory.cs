using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iHome.Jobs.Events.Infrastructure.Models;

[Table("SchedulesRunHistory", Schema = "maciejadmin")]
public class ScheduleRunHistory
{
    [Key]
    public Guid Id { get; set; }
    public Guid ScheduleId { get; set; }
    public DateTime RunDate { get; set; }

}
