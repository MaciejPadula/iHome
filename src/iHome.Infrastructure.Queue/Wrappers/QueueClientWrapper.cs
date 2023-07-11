using Azure.Storage.Queues;
using iHome.Infrastructure.Queue.Models;

namespace iHome.Infrastructure.Queue.Wrappers;

internal class QueueClientWrapper<T> : IQueueClientWrapper<T>
{
    private readonly QueueClient _queueClient;

    public QueueClientWrapper(QueueOptions<T> options)
    {
        _queueClient = new QueueClient(options.ConnectionString, options.QueueName);
    }

    public QueueClient Client => _queueClient;
}
