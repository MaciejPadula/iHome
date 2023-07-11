using iHome.Core.Services;
using iHome.Logic;
using iHome.Models.Requests.Device;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DeviceController : ControllerBase
{
    private readonly IDeviceService _deviceService;
    private readonly IUserAccessor _userAccessor;

    public DeviceController(IDeviceService deviceService, IUserAccessor userAccessor)
    {
        _deviceService = deviceService;
        _userAccessor = userAccessor;
    }

    [HttpPost("AddDevice")]
    public async Task<IActionResult> AddDevice([FromBody] AddDeviceRequest request)
    {
        await _deviceService.AddDevice(
            request.Name, request.MacAddress, request.Type, _userAccessor.UserId, request.RoomId);

        return Ok();
    }

    [HttpPost("ChangeDeviceName")]
    public async Task<IActionResult> ChangeDeviceName([FromBody] ChangeDeviceNameRequest request)
    {
        await _deviceService.ChangeDeviceName(request.DeviceId, request.Name, _userAccessor.UserId);
        return Ok();
    }

    [HttpPost("ChangeDeviceRoom")]
    public async Task<IActionResult> ChangeDeviceRoom([FromBody] ChangeDeviceRoomRequest request)
    {
        await _deviceService.ChangeDeviceRoom(request.DeviceId, request.RoomId, _userAccessor.UserId);
        return Ok();
    }


    [HttpPost("GetDevices")]
    public async Task<IActionResult> GetDevices([FromBody] GetDevicesRequest request)
    {
        var devices = await _deviceService.GetDevices(request.RoomId, _userAccessor.UserId);
        return Ok(devices);
    }

    [HttpPost("GetDevicesForScheduling")]
    public async Task<IActionResult> GetDevicesForScheduling()
    {
        var devices = await _deviceService.GetDevicesForScheduling(_userAccessor.UserId);
        return Ok(devices);
    }

    [HttpPost("RemoveDevice")]
    public async Task<IActionResult> RemoveDevice([FromBody] RemoveDeviceRequest request)
    {
        await _deviceService.RemoveDevice(request.DeviceId, _userAccessor.UserId);
        return Ok();
    }

    [HttpPost("GetDeviceData")]
    public async Task<IActionResult> GetDeviceData([FromBody] GetDeviceDataRequest request)
    {
        var data = await _deviceService.GetDeviceData(request.DeviceId, _userAccessor.UserId);
        return Ok(data);
    }

    [HttpPost("SetDeviceData")]
    public async Task<IActionResult> SetDeviceData([FromBody] SetDeviceDataRequest request)
    {
        await _deviceService.SetDeviceData(request.DeviceId, request.Data, _userAccessor.UserId);
        return Ok();
    }
}
