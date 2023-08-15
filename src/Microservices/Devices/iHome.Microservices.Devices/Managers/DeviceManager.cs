using iHome.Core.Repositories.Devices;
using iHome.Microservices.Devices.Contract.Models;
using iHome.Microservices.Devices.Contract.Models.Request;
using iHome.Microservices.Devices.Contract.Models.Response;
using iHome.Microservices.Devices.Providers;

namespace iHome.Microservices.Devices.Managers;

public class DeviceManager : IDeviceManager
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IDeviceDataSetter _deviceDataSetter;

    public DeviceManager(IDeviceRepository deviceRepository, IDeviceDataSetter deviceDataSetter)
    {
        _deviceRepository = deviceRepository;
        _deviceDataSetter = deviceDataSetter;
    }

    public async Task<GetDeviceResponse> GetDevice(GetDeviceRequest request)
    {
        var device = await _deviceRepository.GetByDeviceId(request.DeviceId);

        return new()
        {
            Device = await _deviceDataSetter.Set(device)
        };
    }

    public async Task<GetDevicesResponse> GetDevices(GetDevicesRequest request)
    {
        IEnumerable<DeviceModel> devices;
        if (request.RoomId == default!)
        {
            devices = await _deviceRepository.GetByUserId(request.UserId);
        }
        else
        {
            devices = await _deviceRepository.GetByRoomId(request.RoomId);
        }

        return new()
        {
            Devices = await _deviceDataSetter.Set(devices.ToList())
        };
    }

    public async Task<GetDevicesResponse> GetDevicesByIds(GetDevicesByIdsRequest request)
    {
        var devices = await _deviceRepository.GetByUserIdAndDeviceIds(request.UserId, request.DeviceIds);

        return new()
        {
            Devices = await _deviceDataSetter.Set(devices.ToList())
        };
    }
}
