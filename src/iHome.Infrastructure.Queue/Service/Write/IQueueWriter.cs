namespace iHome.Infrastructure.Queue.Service.Write;

public interface IQueueWriter<T>
{
    Task Push(T value);
}
