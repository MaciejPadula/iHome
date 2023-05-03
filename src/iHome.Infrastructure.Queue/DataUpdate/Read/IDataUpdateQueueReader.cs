using iHome.Infrastructure.Queue.Models;

namespace iHome.Infrastructure.Queue.DataUpdate.Read
{
    public interface IDataUpdateQueueReader
    {
        Task<DataUpdateModel?> Pop();
        Task<DataUpdateModel?> Peek();

    }
}
