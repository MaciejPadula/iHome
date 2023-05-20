using Firebase.Database;
using iHome.Microservices.Devices.Infrastructure.Models;
using Microsoft.Extensions.Options;

namespace iHome.Microservices.Devices.Infrastructure.Repositories;

public class FirebaseDeviceRepository : IDeviceDataRepository
{
    private readonly FirebaseClient _client;

    public FirebaseDeviceRepository(IOptions<FirebaseSettings> options)
    {
        _client = new FirebaseClient(options.Value.Url);
    }

    public Task<string> GetDeviceData(string macAddess)
    {
        return _client
            .Child($"{macAddess}")
            .OnceAsJsonAsync();
    }

    public Task SetDeviceData(string macAddess, string data)
    {
        return _client
            .Child($"{macAddess}")
            .PutAsync(data);
    }
}
