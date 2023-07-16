using Google.Cloud.Firestore;
using iHome.Infrastructure.Firestore.Serializers;

namespace iHome.Infrastructure.Firestore.Wrappers;

public class FirestoreCollection
{
    private readonly IMessageSerializer _messageSerializer;
    public readonly CollectionReference Collection;

    public FirestoreCollection(CollectionReference collection, IMessageSerializer messageSerializer)
    {
        Collection = collection;
        _messageSerializer = messageSerializer;
    }

    public async Task<string> GetDocumentAsync(string id)
    {
        var doc = Collection.Document(id);
        var snapshot = await doc.GetSnapshotAsync();
        var data = snapshot.ToDictionary();

        return _messageSerializer.Serialize(data);
    }

    public async Task SetDocumentAsync(string id, string data)
    {
        var doc = Collection.Document(id);

        await doc.SetAsync(_messageSerializer.Deserialize(data));
    }
}
