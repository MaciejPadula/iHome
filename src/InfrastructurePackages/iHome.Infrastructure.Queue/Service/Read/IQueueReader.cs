namespace iHome.Infrastructure.Queue.Service.Read;

public interface IQueueReader<T>
{
    Task<T?> Pop();
    Task<T?> Peek();
}
