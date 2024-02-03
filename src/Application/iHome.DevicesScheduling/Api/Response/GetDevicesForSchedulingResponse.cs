using iHome.Model;

namespace iHome.DevicesScheduling.Api.Response;

public class GetDevicesForSchedulingResponse
{
    public required List<DeviceDto> Devices { get; set; }
}
