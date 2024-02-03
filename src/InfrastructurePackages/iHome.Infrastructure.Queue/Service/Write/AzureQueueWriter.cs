using Azure.Storage.Queues;
using iHome.Infrastructure.Queue.Wrappers;
using System.Text.Json;

namespace iHome.Infrastructure.Queue.Service.Write;

internal class AzureQueueWriter<T> : IQueueWriter<T>
{
    private readonly QueueClient _client;

    public AzureQueueWriter(IQueueClientWrapper<T> queueClientWrapper)
    {
        _client = queueClientWrapper.Client;
    }

    public Task Push(T value)
    {
        return _client.SendMessageAsync(JsonSerializer.Serialize(value));
    }
}
