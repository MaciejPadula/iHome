using iHome.Core.Services.Devices;
using iHome.Devices.Contract.Interfaces;
using iHome.Devices.Contract.Models;
using iHome.Devices.Contract.Models.Requests;
using iHome.Logic;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Controllers;
[Route("api/[controller]")]
[ApiController]
public class DeviceController : ControllerBase, IDeviceManipulator, IDeviceProvider
{
    private readonly IDeviceService _deviceService;
    private readonly IUserAccessor _userAccessor;

    public DeviceController(IDeviceService deviceService, IUserAccessor userAccessor)
    {
        _deviceService = deviceService;
        _userAccessor = userAccessor;
    }

    [HttpPost("AddDevice")]
    public Guid AddDevice(AddDeviceRequest request)
    {
        return _deviceService.AddDevice(
            request.Name, request.MacAddress, request.Type, request.HubId,
            request.RoomId, _userAccessor.UserId);
    }

    [HttpPost("ChangeDeviceName")]
    public void ChangeDeviceName(ChangeDeviceNameRequest request)
    {
        _deviceService.RenameDevice(request.DeviceId, request.Name, _userAccessor.UserId);
    }

    [HttpPost("ChangeDeviceRoom")]
    public void ChangeDeviceRoom(ChangeDeviceRoomRequest request)
    {
        _deviceService.ChangeDeviceRoom(request.DeviceId, request.RoomId, _userAccessor.UserId);
    }

    [HttpGet("GetDeviceData/{deviceId}")]
    public string GetDeviceData(Guid deviceId)
    {   
        return _deviceService.GetDevice(deviceId, _userAccessor.UserId).Data;
    }

    [HttpGet("GetDevices/{roomId}")]
    public IEnumerable<Device> GetDevices(Guid roomId)
    {
        return _deviceService.GetDevices(_userAccessor.UserId)
            .Where(d => d.RoomId == roomId)
            .ToList();
    }

    [HttpDelete("RemoveDevice")]
    public void RemoveDevice(RemoveDeviceRequest request)
    {
        _deviceService.RemoveDevice(request.DeviceId, _userAccessor.UserId);
    }

    [HttpPost("SetDeviceData")]
    public void SetDeviceData(SetDeviceDataRequest request)
    {
        _deviceService.SetDeviceData(request.DeviceId, request.Data, _userAccessor.UserId);
    }
}
