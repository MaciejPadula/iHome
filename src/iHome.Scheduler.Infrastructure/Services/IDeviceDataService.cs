using iHome.Scheduler.Infrastructure.Models;

namespace iHome.Scheduler.Infrastructure.Services;

public interface IDeviceDataService
{
    Task UpdateDeviceData(UpdateDeviceDataRequest request);
}
