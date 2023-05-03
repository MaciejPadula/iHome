using iHome.Infrastructure.Queue.DataUpdate.Read;
using iHome.Infrastructure.Queue.DataUpdate.Write;
using iHome.Infrastructure.Queue.Models;

namespace iHome.Infrastructure.Queue.DataUpdate
{
    public interface IDataUpdateQueue : IDataUpdateQueueReader, IDataUpdateQueueWriter
    {
    }

    public class DataUpdateQueue : IDataUpdateQueue
    {
        private readonly IDataUpdateQueueReader _reader;
        private readonly IDataUpdateQueueWriter _writer;

        public DataUpdateQueue(IDataUpdateQueueReader reader, IDataUpdateQueueWriter writer)
        {
            _reader = reader;
            _writer = writer;
        }

        public Task<DataUpdateModel?> Peek()
        {
            return _reader.Peek();
        }

        public Task<DataUpdateModel?> Pop()
        {
            return _reader.Pop();
        }

        public Task Push(DataUpdateModel value)
        {
            return _writer.Push(value);
        }
    }
}
