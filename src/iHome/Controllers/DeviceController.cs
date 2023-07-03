using iHome.Core.Services;
using iHome.Logic;
using iHome.Microservices.Devices.Contract;
using iHome.Microservices.Devices.Contract.Models;
using iHome.Models.Requests.Device;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DeviceController : ControllerBase
{
    private readonly IDeviceManagementService _deviceManagementService;
    private readonly IDeviceDataService _deviceDataService;
    private readonly IDevicesForSchedulingAccessor _devicesForSchedulingAccessor;
    private readonly IUserAccessor _userAccessor;

    public DeviceController(IDeviceManagementService deviceManagementService, IDeviceDataService deviceDataService, IDevicesForSchedulingAccessor devicesForSchedulingAccessor, IUserAccessor userAccessor)
    {
        _deviceManagementService = deviceManagementService;
        _deviceDataService = deviceDataService;
        _devicesForSchedulingAccessor = devicesForSchedulingAccessor;
        _userAccessor = userAccessor;
    }

    [HttpPost("AddDevice")]
    public async Task<IActionResult> AddDevice([FromBody] AddDeviceRequest request)
    {
        var deviceId = await _deviceManagementService.AddDevice(new()
        {
            Name = request.Name,
            MacAddress = request.MacAddress,
            Type = request.Type,
            RoomId = request.RoomId
        });

        return Ok(deviceId.DeviceId);
    }

    [HttpPost("ChangeDeviceName")]
    public async Task<IActionResult> ChangeDeviceName([FromBody] ChangeDeviceNameRequest request)
    {
        await _deviceManagementService.RenameDevice(new()
        {
            DeviceId = request.DeviceId,
            NewName = request.Name
        });
        return Ok();
    }

    [HttpPost("ChangeDeviceRoom")]
    public async Task<IActionResult> ChangeDeviceRoom([FromBody] ChangeDeviceRoomRequest request)
    {
        await _deviceManagementService.ChangeDeviceRoom(new()
        {
            DeviceId = request.DeviceId,
            RoomId = request.RoomId
        });
        return Ok();
    }


    [HttpPost("GetDevices")]
    public async Task<IActionResult> GetDevices([FromBody] GetDevicesRequest request)
    {
        var devices = await _deviceManagementService.GetDevices(new()
        {
            RoomId = request.RoomId ?? default!,
            UserId = _userAccessor.UserId
        });

        return Ok(devices.Devices);
    }

    [HttpPost("GetDevicesForScheduling")]
    public async Task<IActionResult> GetDevicesForScheduling()
    {
        var devices = await _devicesForSchedulingAccessor.Get(_userAccessor.UserId);

        return Ok(devices);
    }

    [HttpPost("RemoveDevice")]
    public async Task<IActionResult> RemoveDevice([FromBody] RemoveDeviceRequest request)
    {
        await _deviceManagementService.RemoveDevice(new()
        {
            DeviceId = request.DeviceId
        });

        return Ok();
    }

    [HttpPost("GetDeviceData")]
    public async Task<IActionResult> GetDeviceData([FromBody] GetDeviceDataRequest request)
    {
        var data = await _deviceDataService.GetDeviceData(new()
        {
            DeviceId = request.DeviceId
        });

        return Ok(data.DeviceData);
    }

    [HttpPost("SetDeviceData")]
    public async Task<IActionResult> SetDeviceData([FromBody] SetDeviceDataRequest request)
    {
        await _deviceDataService.SetDeviceData(new()
        {
            DeviceId = request.DeviceId,
            Data = request.Data
        });

        return Ok();
    }
}
