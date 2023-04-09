using iHome.Infrastructure.SQL.Models;

namespace iHome.Models.Responses;

public class GetWidgetsWidget
{
    public required Guid Id { get; set; }
    public required WidgetType WidgetType { get; set; }
    public required Guid RoomId { get; set; }
    public required bool ShowBorder { get; set; }
    public required int MaxNumberOfDevices { get; set; }
}
