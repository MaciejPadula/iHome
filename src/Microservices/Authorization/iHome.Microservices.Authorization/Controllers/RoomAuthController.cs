using iHome.Microservices.Authorization.Contract;
using iHome.Microservices.Authorization.Contract.Models.Request;
using iHome.Microservices.Authorization.Contract.Models.Response;
using iHome.Microservices.Authorization.Managers;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Microservices.Authorization.Controllers
{
    public class RoomAuthController : ControllerBase, IRoomAuthService
    {
        private readonly IRoomManager _roomManager;

        public RoomAuthController(IRoomManager roomManager)
        {
            _roomManager = roomManager;
        }

        [HttpPost]
        public async Task<AuthResponse> CanReadRoom([FromBody] RoomAuthRequest request)
        {
            return new() 
            {
                HasAccess = await _roomManager.CanRead(request)
            };
        }

        [HttpPost]
        public async Task<AuthResponse> CanWriteRoom([FromBody] RoomAuthRequest request)
        {
            return new()
            {
                HasAccess = await _roomManager.CanWrite(request)
            };
        }
    }
}
