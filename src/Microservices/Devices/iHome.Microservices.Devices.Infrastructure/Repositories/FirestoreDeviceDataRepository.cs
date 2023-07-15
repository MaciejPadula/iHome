using iHome.Microservices.Devices.Infrastructure.Logic;

namespace iHome.Microservices.Devices.Infrastructure.Repositories;

public class FirestoreDeviceDataRepository : IDeviceDataRepository
{
    private readonly IFirestoreConnectionFactory _connectionFactory;

    private const string CollectionPath = "devices";

    public FirestoreDeviceDataRepository(IFirestoreConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<string> GetDeviceData(string macAddess)
    {
        var conn = await _connectionFactory.GetFirestoreConnection();

        var coll = conn.Collection(CollectionPath);
        var doc = coll.Document(macAddess);

        var snap = await doc.GetSnapshotAsync();

        return snap.GetValue<string>("");
    }

    public async Task SetDeviceData(string macAddess, string data)
    {
        var conn = await _connectionFactory.GetFirestoreConnection();

        var collection = conn.Collection(CollectionPath);
        var deviceDocument = collection.Document(macAddess);

        await deviceDocument.SetAsync(data);
    }
}
