namespace iHome.Infrastructure.Queue.Service.Read;

internal class ListQueueReader<T> : IQueueReader<T>
{
    private readonly List<T> _queue;

    public ListQueueReader(List<T> queue)
    {
        _queue = queue;
    }

    public async Task<T?> Pop()
    {
        var value = await Peek();
        if(value != null)
        {
            _queue.RemoveAt(0);
        }

        return value;
    }

    public Task<T?> Peek()
    {
        var value = _queue.FirstOrDefault();
        return Task.FromResult(value);
    }
}
