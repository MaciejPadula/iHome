using Firebase.Database;

namespace iHome.Core.Repositories;

public interface IDeviceDataRepository
{
    Task<string> GetData(string identifier);
    Task SetData(string identifier, string data);
}

public class FirebaseDeviceDataRepository : IDeviceDataRepository
{
    private readonly FirebaseClient _client;

    public FirebaseDeviceDataRepository(FirebaseClient client)
    {
        _client = client;
    }

    public async Task<string> GetData(string identifier)
    {
        var data = await _client
            .Child($"{identifier}")
            .OnceAsJsonAsync();

        return data;
    }

    public async Task SetData(string identifier, string data)
    {
        await _client
            .Child($"{identifier}")
            .PutAsync(data);
    }
}
