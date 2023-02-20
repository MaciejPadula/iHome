using iHome.Devices.Contract.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iHome.Core.Models;

[Table("WidgetsDevices", Schema = "maciejadmin")]
public class WidgetDevice
{
    [Key]
    public Guid Id { get; set; }
    public Guid WidgetId { get; set; }
    public Guid DeviceId { get; set; }

    public virtual Device Device { get; set; }
}
