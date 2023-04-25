using iHome.Core.Services.Devices;
using iHome.Devices.Contract.Interfaces;
using iHome.Devices.Contract.Models.Requests;
using iHome.Infrastructure.SQL.Models;
using iHome.Logic;
using iHome.Models.Requests;
using iHome.Models.Responses;
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
    public async Task<IEnumerable<GetDevicesDevice>> GetDevices([FromBody] GetDevicesRequest request)
    {
        var devices = new List<Device>();
        if(request.RoomId.HasValue)
        {
            devices.AddRange(await _deviceService.GetDevices(request.RoomId.Value, _userAccessor.UserId));
        }
        else
        {
            devices.AddRange(await _deviceService.GetDevices(_userAccessor.UserId));
        }

        return devices.Select(d => new GetDevicesDevice(d));
    }

    private readonly List<DeviceType> _devicesForScheduling = new()
    {
        DeviceType.RGBLamp
    };

    [HttpPost("GetDevicesForScheduling")]
    public async Task<IEnumerable<GetDevicesDevice>> GetDevicesForScheduling()
    {
        var devices = await _deviceService.GetDevices(_userAccessor.UserId);

        return devices
            .Where(d => _devicesForScheduling.Any(type => d.Type == type))
            .Select(d => new GetDevicesDevice(d));
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
