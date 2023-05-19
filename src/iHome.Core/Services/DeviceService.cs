using iHome.Core.Exceptions.SqlExceptions;
using iHome.Core.Logic.ActionValidators;
using iHome.Core.Models;
using iHome.Core.Repositories.Devices;
using iHome.Infrastructure.Firebase.Repositories;
using iHome.Infrastructure.SQL.Models.Enums;

namespace iHome.Core.Services;

public interface IDeviceService
{
    Task<Guid> AddDevice(string name, string macAddress, DeviceType type, Guid roomId, string userId);
    Task<DeviceModel> GetDevice(Guid deviceId, string userId);
    Task<IEnumerable<DeviceModel>> GetDevices(Guid roomId, string userId);
    Task<IEnumerable<DeviceModel>> GetDevices(string userId);
    Task RemoveDevice(Guid deviceId, string userId);
    Task RenameDevice(Guid deviceId, string newName, string userId);
    Task ChangeDeviceRoom(Guid deviceId, Guid roomId, string userId);
    Task SetDeviceData(Guid deviceId, string data, string userId);
    Task<string> GetDeviceData(Guid deviceId, string userId);
}

public class DeviceService : IDeviceService
{
    private readonly IDeviceActionValidator _deviceActionValidator;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IDeviceDataRepository _deviceDataRepository;

    public DeviceService(IDeviceActionValidator deviceActionValidator, IDeviceRepository deviceRepository, IDeviceDataRepository deviceDataRepository)
    {
        _deviceActionValidator = deviceActionValidator;
        _deviceRepository = deviceRepository;
        _deviceDataRepository = deviceDataRepository;
    }

    public async Task<Guid> AddDevice(string name, string macAddress, DeviceType type, Guid roomId, string userId)
    {
        _deviceActionValidator.ValidateAddAndThrow(roomId, macAddress, userId);

        return await _deviceRepository.Add(name, macAddress, type, roomId);
    }

    public async Task ChangeDeviceRoom(Guid deviceId, Guid roomId, string userId)
    {
        var device = await GetDevice(deviceId, userId);

        _deviceActionValidator.ValidateRemoveAndThrow(device, userId);

        await _deviceRepository.ChangeRoom(deviceId, roomId);
    }

    public async Task<DeviceModel> GetDevice(Guid deviceId, string userId)
    {
        var device = (await _deviceRepository.GetByDeviceId(deviceId)).FirstOrDefault() ?? throw new DeviceNotFoundException();
        _deviceActionValidator.ValidateRemoveAndThrow(device, userId);

        return device;
    }

    public async Task<IEnumerable<DeviceModel>> GetDevices(Guid roomId, string userId)
    {
        _deviceActionValidator.ValidateGetAndThrow(roomId, userId);

        return await _deviceRepository.GetByRoomId(roomId);
    }

    public async Task<IEnumerable<DeviceModel>> GetDevices(string userId)
    {
        return await _deviceRepository.GetByUserId(userId);
    }

    public async Task RemoveDevice(Guid deviceId, string userId)
    {
        var device = (await _deviceRepository.GetByDeviceId(deviceId)).FirstOrDefault() ?? throw new DeviceNotFoundException();

        _deviceActionValidator.ValidateRemoveAndThrow(device, userId);

        await _deviceRepository.Remove(device.Id);
    }

    public async Task RenameDevice(Guid deviceId, string newName, string userId)
    {
        await _deviceRepository.Rename(deviceId, newName);
    }

    public async Task SetDeviceData(Guid deviceId, string data, string userId)
    {
        var device = await GetDevice(deviceId, userId);

        await _deviceDataRepository.SetData(device.MacAddress, data);
    }

    public async Task<string> GetDeviceData(Guid deviceId, string userId)
    {
        var device = await GetDevice(deviceId, userId);

        return await _deviceDataRepository.GetData(device.MacAddress);
    }
}
