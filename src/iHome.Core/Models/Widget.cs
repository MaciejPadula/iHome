using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iHome.Core.Models;

[Table("Widgets", Schema = "maciejadmin")]
public class Widget
{
    [Key]
    public Guid Id { get; set; }
    public WidgetType WidgetType { get; set; }
    public Guid RoomId { get; set; }
    public bool ShowBorder { get; set; }
}
