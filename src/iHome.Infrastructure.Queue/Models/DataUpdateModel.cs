namespace iHome.Infrastructure.Queue.Models;

public class DataUpdateModel
{
    public Guid DeviceId { get; set; } = default!;
    public string DeviceData { get; set; } = default!;
}
