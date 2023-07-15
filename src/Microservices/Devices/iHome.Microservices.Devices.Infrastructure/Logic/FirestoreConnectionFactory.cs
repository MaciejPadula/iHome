using Google.Cloud.Firestore;
using iHome.Microservices.Devices.Infrastructure.Models;

namespace iHome.Microservices.Devices.Infrastructure.Logic;

public interface IFirestoreConnectionFactory
{
    Task<FirestoreDb> GetFirestoreConnection();
}

public class FirestoreConnectionFactory : IFirestoreConnectionFactory
{
    private readonly string _projectId;

    public FirestoreConnectionFactory(FirestoreConfiguration options)
    {
        _projectId = options.ProjectId;
    }

    public async Task<FirestoreDb> GetFirestoreConnection()
    {
        return await FirestoreDb.CreateAsync(_projectId);
    }
}
