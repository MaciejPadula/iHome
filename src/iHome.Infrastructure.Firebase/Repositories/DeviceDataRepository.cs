using Firebase.Database;

namespace iHome.Infrastructure.Firebase.Repositories;

public interface IDeviceDataRepository
{
    string GetData(string identifier);
    void SetData(string identifier, string data);
}

public class FirebaseDeviceDataRepository : IDeviceDataRepository
{
    private readonly FirebaseClient _client;

    public FirebaseDeviceDataRepository(FirebaseClient client)
    {
        _client = client;
    }

    public string GetData(string identifier)
    {
        return _client
            .Child($"{identifier}")
            .OnceAsJsonAsync()
            .Result;
    }

    public void SetData(string identifier, string data)
    {
        _client
            .Child($"{identifier}")
            .PutAsync(data);
    }
}
