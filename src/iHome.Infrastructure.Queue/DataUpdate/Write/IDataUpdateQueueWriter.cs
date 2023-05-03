using iHome.Infrastructure.Queue.Models;

namespace iHome.Infrastructure.Queue.DataUpdate.Write
{
    public interface IDataUpdateQueueWriter
    {
        Task Push(DataUpdateModel value);
    }
}
