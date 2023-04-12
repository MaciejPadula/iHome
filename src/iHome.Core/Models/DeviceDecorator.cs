using iHome.Infrastructure.SQL.Models;

namespace iHome.Core.Models;

public class DeviceDecorator
{
    public required Device Device { get; set; }
    public required ScheduleDevice ScheduleDevice { get; set; }
}
