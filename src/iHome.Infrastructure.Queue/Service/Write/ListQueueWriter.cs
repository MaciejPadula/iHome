namespace iHome.Infrastructure.Queue.Service.Write;

internal class ListQueueWriter<T> : IQueueWriter<T>
{
    private readonly List<T> _queue;

    public ListQueueWriter(List<T> queue)
    {
        _queue = queue;
    }

    public Task Push(T value)
    {
        _queue.Add(value);
        return Task.CompletedTask;
    }
}
