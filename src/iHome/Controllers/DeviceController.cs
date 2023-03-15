using iHome.Core.Services.Devices;
using iHome.Devices.Contract.Interfaces;
using iHome.Devices.Contract.Models;
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
    private readonly IUserAccessor _userAccessor;

    public DeviceController(IDeviceService deviceService, IUserAccessor userAccessor)
    {
        _deviceService = deviceService;
        _userAccessor = userAccessor;
    }

    [HttpPost("AddDevice")]
    public Guid AddDevice([FromBody] AddDeviceRequest request)
    {
        return _deviceService.AddDevice(
            request.Name, request.MacAddress, request.Type,
            request.RoomId, _userAccessor.UserId);
    }

    [HttpPost("ChangeDeviceName")]
    public void ChangeDeviceName([FromBody] ChangeDeviceNameRequest request)
    {
        _deviceService.RenameDevice(request.DeviceId, request.Name, _userAccessor.UserId);
    }

    [HttpPost("ChangeDeviceRoom")]
    public void ChangeDeviceRoom([FromBody] ChangeDeviceRoomRequest request)
    {
        _deviceService.ChangeDeviceRoom(request.DeviceId, request.RoomId, _userAccessor.UserId);
    }

    [HttpPost("GetDeviceData")]
    public string GetDeviceData([FromBody] GetDeviceDataRequest request)
    {
        return _deviceService.GetDeviceData(request.DeviceId, _userAccessor.UserId);
    }

    [HttpPost("GetDevices")]
    public IEnumerable<Device> GetDevices([FromBody] GetDevicesRequest request)
    {
        return _deviceService.GetDevices(request.RoomId, _userAccessor.UserId);
    }

    [HttpPost("RemoveDevice")]
    public void RemoveDevice([FromBody] RemoveDeviceRequest request)
    {
        _deviceService.RemoveDevice(request.DeviceId, _userAccessor.UserId);
    }

    [HttpPost("SetDeviceData")]
    public void SetDeviceData([FromBody] SetDeviceDataRequest request)
    {
        _deviceService.SetDeviceData(request.DeviceId, request.Data, _userAccessor.UserId);
    }
}
