using iHome.Scheduler.Infrastructure.Models;

namespace iHome.Scheduler.Infrastructure.Services;

public class FirebaseDeviceDataService : IDeviceDataService
{
    public Task UpdateDeviceData(UpdateDeviceDataRequest request)
    {
        return Task.CompletedTask;
    }
}
