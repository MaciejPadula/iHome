using iHome.Core.Models;
using iHome.Core.Services.Devices;
using iHome.Devices.Contract.Interfaces;
using iHome.Devices.Contract.Models.Requests;
using iHome.Logic;
using iHome.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DeviceController : ControllerBase, IDeviceDataService
{
    private readonly IDeviceService _deviceService;
    private readonly IDevicesForSchedulingAccessor _devicesForSchedulingAccessor;
    private readonly IUserAccessor _userAccessor;

    public DeviceController(IDeviceService deviceService, IDevicesForSchedulingAccessor devicesForSchedulingAccessor, IUserAccessor userAccessor)
    {
        _deviceService = deviceService;
        _devicesForSchedulingAccessor = devicesForSchedulingAccessor;
        _userAccessor = userAccessor;
    }

    [HttpPost("AddDevice")]
    public async Task<Guid> AddDevice([FromBody] AddDeviceRequest request)
    {
        var deviceId = await _deviceService.AddDevice(
            request.Name, request.MacAddress, request.Type,
            request.RoomId, _userAccessor.UserId);

        return deviceId;
    }

    [HttpPost("ChangeDeviceName")]
    public async Task ChangeDeviceName([FromBody] ChangeDeviceNameRequest request)
    {
        await _deviceService.RenameDevice(request.DeviceId, request.Name, _userAccessor.UserId);
    }

    [HttpPost("ChangeDeviceRoom")]
    public async Task ChangeDeviceRoom([FromBody] ChangeDeviceRoomRequest request)
    {
        await _deviceService.ChangeDeviceRoom(request.DeviceId, request.RoomId, _userAccessor.UserId);
    }

    [HttpPost("GetDeviceData")]
    public async Task<string> GetDeviceData([FromBody] GetDeviceDataRequest request)
    {
        var data = await _deviceService.GetDeviceData(request.DeviceId, _userAccessor.UserId);

        return data;
    }

    [HttpPost("GetDevices")]
    public async Task<IEnumerable<DeviceModel>> GetDevices([FromBody] GetDevicesRequest request)
    {
        var devices = new List<DeviceModel>();
        if (request.RoomId.HasValue)
        {
            devices.AddRange(await _deviceService.GetDevices(request.RoomId.Value, _userAccessor.UserId));
        }
        else
        {
            devices.AddRange(await _deviceService.GetDevices(_userAccessor.UserId));
        }

        return devices;
    }

    [HttpPost("GetDevicesForScheduling")]
    public async Task<IEnumerable<DeviceModel>> GetDevicesForScheduling()
    {
        var devices = await _devicesForSchedulingAccessor.Get(_userAccessor.UserId);

        return devices;
    }

    [HttpPost("RemoveDevice")]
    public async Task RemoveDevice([FromBody] RemoveDeviceRequest request)
    {
        await _deviceService.RemoveDevice(request.DeviceId, _userAccessor.UserId);
    }

    [HttpPost("SetDeviceData")]
    public async Task SetDeviceData([FromBody] SetDeviceDataRequest request)
    {
        await _deviceService.SetDeviceData(request.DeviceId, request.Data, _userAccessor.UserId);
    }
}
