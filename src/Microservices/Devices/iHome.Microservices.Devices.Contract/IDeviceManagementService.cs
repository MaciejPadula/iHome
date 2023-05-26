using iHome.Microservices.Devices.Contract.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using iHome.Microservices.Devices.Contract.Models.Request;
using iHome.Microservices.Devices.Contract.Models.Response;

namespace iHome.Microservices.Devices.Contract
{
    public interface IDeviceManagementService
    {
        Task<AddDeviceResponse> AddDevice(AddDeviceRequest request);

        Task<GetDeviceResponse> GetDevice(GetDeviceRequest request);
        Task<GetDevicesResponse> GetDevices(GetDevicesRequest request);

        Task RemoveDevice(RemoveDeviceRequest request);

        Task RenameDevice(RenameDeviceRequest request);
        Task ChangeDeviceRoom(ChangeDeviceRoomRequest request);

    }
}
