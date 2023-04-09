using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iHome.Infrastructure.SQL.Models;

[Table("Widgets", Schema = "maciejadmin")]
public class Widget
{
    [Key]
    public Guid Id { get; set; }
    public WidgetType WidgetType { get; set; }
    public Guid RoomId { get; set; }
    public bool ShowBorder { get; set; }
    public virtual Collection<WidgetDevice> WidgetDevices { get; set; } = new();

    [NotMapped]
    public int MaxNumberOfDevices => GetMaxNumberOfDevices(WidgetType);

    private int GetMaxNumberOfDevices(WidgetType type) => type switch
    {
        WidgetType.Unknown => 0,
        WidgetType.Small => 1,
        WidgetType.Medium => 2,
        WidgetType.Wide => 4,
        _ => 0
    };
}
