using Azure.Storage.Queues;
using System.Text.Json;

namespace iHome.Infrastructure.Queue.Service.Write;

internal class AzureQueueWriter<T> : IQueueWriter<T>
{
    private readonly QueueClient _client;

    public AzureQueueWriter(QueueClient client)
    {
        _client = client;
    }

    public Task Push(T value)
    {
        return _client.SendMessageAsync(JsonSerializer.Serialize(value));
    }
}
