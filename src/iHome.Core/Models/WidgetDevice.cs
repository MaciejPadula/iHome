using System.ComponentModel.DataAnnotations.Schema;

namespace iHome.Core.Models;

[Table("WidgetsDevices", Schema = "maciejadmin")]
public class WidgetDevice
{
    public Guid Id { get; set; }
    public Guid WidgetId { get; set; }
    public Guid DeviceId { get; set; }
}
