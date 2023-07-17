using Google.Cloud.Firestore;
using iHome.Infrastructure.Firestore.Serializers;
using iHome.Infrastructure.Firestore.Wrappers;
using iHome.Microservices.Devices.Infrastructure.Models;
using Newtonsoft.Json;

namespace iHome.Microservices.Devices.Infrastructure.Logic;

public interface IFirestoreConnectionFactory
{
    Task<FirestoreDbConnection> GetFirestoreConnection();
}

public class FirestoreConnectionFactory : IFirestoreConnectionFactory
{
    private readonly IMessageSerializer _messageSerializer;
    private readonly string _projectId;
    private readonly string _jsonCredentials;

    public FirestoreConnectionFactory(FirestoreOptions options, IMessageSerializer messageSerializer)
    {
        _projectId = options.ProjectId;
        _jsonCredentials = JsonConvert.SerializeObject(options.JsonCredentials);
        _messageSerializer = messageSerializer;
    }

    public async Task<FirestoreDbConnection> GetFirestoreConnection()
    {
        var db = await new FirestoreDbBuilder
        {
            ProjectId = _projectId,
            JsonCredentials = _jsonCredentials
        }.BuildAsync();

        return new FirestoreDbConnection(db, _messageSerializer);
    }
}
