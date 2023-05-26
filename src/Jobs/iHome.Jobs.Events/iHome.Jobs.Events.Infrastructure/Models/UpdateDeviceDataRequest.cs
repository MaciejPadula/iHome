namespace iHome.Jobs.Events.Infrastructure.Models;

public class UpdateDeviceDataRequest
{
    public required Guid DeviceId { get; set; }
    public required string DeviceData { get; set; }
}
