using Azure.Storage.Queues;

namespace iHome.Infrastructure.Queue.Wrappers;

public interface IQueueClientWrapper<T>
{
    QueueClient Client { get; }
}
