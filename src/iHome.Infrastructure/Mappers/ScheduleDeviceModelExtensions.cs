using iHome.Microservices.Schedules.Contract.Models;
using iHome.Model;

namespace iHome.Infrastructure.Mappers;

internal static class ScheduleDeviceModelExtensions
{
    internal static ScheduleDeviceDto ToDto(this ScheduleDeviceModel device)
    {
        return new ScheduleDeviceDto
        {
            Id = device.Id,
            DeviceId = device.DeviceId,
            ScheduleId = device.ScheduleId,
            Data = device.DeviceData,
            Name = device.Name,
            Type = device.Type,
        };
    }
}
