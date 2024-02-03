namespace iHome.Model;

public class DeviceDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required int Type { get; set; }
}
