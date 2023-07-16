namespace iHome.Infrastructure.Firestore.Serializers;

public interface IMessageSerializer
{
    string Serialize(Dictionary<string, object> obj);
    Dictionary<string, object> Deserialize(string data);
}
