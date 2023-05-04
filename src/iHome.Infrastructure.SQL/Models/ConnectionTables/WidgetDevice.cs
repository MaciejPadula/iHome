using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iHome.Infrastructure.SQL.Models.RootTables;

namespace iHome.Infrastructure.SQL.Models.ConnectionTables;

[Table("WidgetsDevices", Schema = "maciejadmin")]
public class WidgetDevice
{
    [Key]
    public Guid Id { get; set; }
    public Guid WidgetId { get; set; }
    public Guid DeviceId { get; set; }

    public virtual Device? Device { get; set; }
}
