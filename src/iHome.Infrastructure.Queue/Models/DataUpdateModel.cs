namespace iHome.Infrastructure.Queue.Models;

public class DataUpdateModel
{
    public string MacAddress { get; set; } = default!;
    public string DeviceData { get; set; } = default!;
}
