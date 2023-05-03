using iHome.Infrastructure.Queue.Service.Read;
using iHome.Infrastructure.Queue.Service.Write;

namespace iHome.Infrastructure.Queue.Service
{
    public interface IDataUpdateQueue<T> : IQueueReader<T>, IQueueWriter<T>
    {
    }

    public class DataUpdateQueue<T> : IDataUpdateQueue<T>
    {
        private readonly IQueueReader<T> _reader;
        private readonly IQueueWriter<T> _writer;

        public DataUpdateQueue(IQueueReader<T> reader, IQueueWriter<T> writer)
        {
            _reader = reader;
            _writer = writer;
        }

        public Task<T?> Peek()
        {
            return _reader.Peek();
        }

        public Task<T?> Pop()
        {
            return _reader.Pop();
        }

        public Task Push(T value)
        {
            return _writer.Push(value);
        }
    }
}
