using Google.Cloud.Firestore;
using iHome.Infrastructure.Firestore.Serializers;

namespace iHome.Infrastructure.Firestore.Wrappers;

public class FirestoreDbConnection
{
    public readonly FirestoreDb Db;
    private readonly IMessageSerializer _messageSerializer;

    public FirestoreDbConnection(FirestoreDb db, IMessageSerializer messageSerializer)
    {
        Db = db;
        _messageSerializer = messageSerializer;
    }

    public FirestoreCollection Collection(string collectionName)
    {
        return new FirestoreCollection(Db.Collection(collectionName), _messageSerializer);
    }
}
