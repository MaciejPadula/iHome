using iHome.Microservices.RoomsManagement.Contract;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Microservices.RoomsManagement.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RoomAccessController : ControllerBase, IRoomAccessService
    {
        public Task<bool> RoomAlreadyExists(string name, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RoomAlreadyShared(Guid roomId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UserHasReadAccess(Guid roomId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UserHasWriteAccess(Guid roomId, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
