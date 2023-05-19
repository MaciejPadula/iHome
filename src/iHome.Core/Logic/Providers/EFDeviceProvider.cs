using iHome.Core.Models;
using iHome.Infrastructure.SQL.Contexts;
using Microsoft.EntityFrameworkCore;

namespace iHome.Core.Logic.Providers;

public class EFDeviceProvider : IDeviceProvider
{
    private readonly SqlDataContext _sqlDataContext;

    public EFDeviceProvider(SqlDataContext sqlDataContext)
    {
        _sqlDataContext = sqlDataContext;
    }

    public async Task<DeviceModel?> Get(Guid deviceId)
    {
        return await _sqlDataContext.Devices
            .Where(d => d.Id == deviceId)
            .Select(d => new DeviceModel(d))
            .FirstOrDefaultAsync();
    }
}
