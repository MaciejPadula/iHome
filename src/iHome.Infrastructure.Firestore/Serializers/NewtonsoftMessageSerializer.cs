using Newtonsoft.Json;

namespace iHome.Infrastructure.Firestore.Serializers;

public class NewtonsoftMessageSerializer : IMessageSerializer
{
    public string Serialize(Dictionary<string, object> obj) =>
        JsonConvert.SerializeObject(obj);

    public Dictionary<string, object> Deserialize(string data) =>
        JsonConvert.DeserializeObject<Dictionary<string, object>>(data) ?? new();
}
