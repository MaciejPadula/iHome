using iHome.Model;

namespace iHome.DevicesScheduling.Api.Response;

public class GetScheduleDevicesResponse
{
    public required List<ScheduleDeviceDto> ScheduleDevices { get; set; }
}
