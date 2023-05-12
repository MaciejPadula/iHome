using iHome.Core.Models;
using iHome.Infrastructure.Firebase.Repositories;
using iHome.Infrastructure.SQL.Contexts;
using iHome.Infrastructure.SQL.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace iHome.Core.Services.Devices;

public class DevicesForSchedulingAccessor : IDevicesForSchedulingAccessor
{
    private readonly IDeviceDataRepository _deviceDataRepository;
    private readonly SqlDataContext _sqlDataContext;

    private readonly List<DeviceType> _devicesForScheduling = new()
    {
        DeviceType.RGBLamp,
        DeviceType.RobotVaccumCleaner
    };

    public DevicesForSchedulingAccessor(IDeviceDataRepository deviceDataRepository, SqlDataContext sqlDataContext)
    {
        _deviceDataRepository = deviceDataRepository;
        _sqlDataContext = sqlDataContext;
    }

    public async Task<IEnumerable<DeviceModel>> Get(string userId)
    {
        var devices = await _sqlDataContext.Devices
            .Where(d => _devicesForScheduling.Contains(d.Type))
            .Include(d => d.Room)
            .Where(d => d.Room != null && d.Room.UserId == userId)
            .Select(d => new DeviceModel(d))
            .ToListAsync();

        foreach (var device in devices)
        {
            device.Data = await _deviceDataRepository.GetData(device.MacAddress);
        }

        return devices;
    }
}
