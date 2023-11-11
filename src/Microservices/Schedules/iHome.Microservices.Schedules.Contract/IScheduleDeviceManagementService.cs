using iHome.Microservices.Schedules.Contract.Models.Request;
using iHome.Microservices.Schedules.Contract.Models.Response;
using System.Threading.Tasks;

namespace iHome.Microservices.Schedules.Contract
{
    public interface IScheduleDeviceManagementService
    {
        Task<AddOrUpdateDeviceScheduleResponse> AddOrUpdateDeviceSchedule(AddOrUpdateDeviceScheduleRequest request);

        Task<GetScheduleDeviceResponse> GetScheduleDevice(GetScheduleDeviceRequest request);
        Task<GetScheduleDevicesResponse> GetScheduleDevices(GetScheduleDevicesRequest request);
        Task<GetDevicesForSchedulingResponse> GetDevicesForScheduling(GetDevicesForSchedulingRequest request);

        Task<GetDevicesInScheduleCountResponse> GetDevicesInScheduleCount(GetDevicesInScheduleCountRequest request);

        Task RemoveDeviceSchedule(RemoveDeviceScheduleRequest request);
    }
}
