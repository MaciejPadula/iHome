using System;
using System.Threading.Tasks;

namespace iHome.Microservices.RoomsManagement.Contract
{
    public interface IRoomAccessService
    {
        Task<bool> UserHasWriteAccess(Guid roomId, string userId);
        Task<bool> UserHasReadAccess(Guid roomId, string userId);

        Task<bool> RoomAlreadyExists(string name, string userId);
        Task<bool> RoomAlreadyShared(Guid roomId, string userId);
    }
}