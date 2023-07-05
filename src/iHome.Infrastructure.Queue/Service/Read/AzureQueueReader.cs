using Azure.Storage.Queues;
using iHome.Infrastructure.Queue.Exceptions;
using iHome.Infrastructure.Queue.Wrappers;

namespace iHome.Infrastructure.Queue.Service.Read;

internal class AzureQueueReader<T> : IQueueReader<T>
{
    private readonly QueueClient _client;

    public AzureQueueReader(IQueueClientWrapper<T> queueClientWrapper)
    {
        _client = queueClientWrapper.Client;
    }

    public async Task<T?> Peek()
    {
        var response = await _client.PeekMessageAsync();
        if (response?.Value?.Body == null) return default;

        return response.Value.Body.ToObjectFromJson<T>();
    }

    public async Task<T?> Pop()
    {
        var response = await _client.ReceiveMessageAsync();
        if (response?.Value?.Body == null) throw new QueueEmptyException();

        var data = response.Value.Body.ToObjectFromJson<T>();


        await _client.DeleteMessageAsync(response?.Value.MessageId, response?.Value.PopReceipt);
        return data;
    }
}
