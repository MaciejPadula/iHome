using iHome.Microservices.RoomsManagement.Contract;
using iHome.Microservices.RoomsManagement.Contract.Models.Request;
using iHome.Microservices.RoomsManagement.Contract.Models.Response;
using iHome.Microservices.RoomsManagement.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Microservices.RoomsManagement.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RoomManagementController : ControllerBase, IRoomManagementService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IUserRoomRepository _userRoomRepository;

        public RoomManagementController(IRoomRepository roomRepository, IUserRoomRepository userRoomRepository)
        {
            _roomRepository = roomRepository;
            _userRoomRepository = userRoomRepository;
        }

        public Task AddRoom(AddRoomRequest request)
        {
            return _roomRepository.Add(request.RoomName, request.UserId);
        }

        public async Task<GetRoomsResponse> GetRooms(GetRoomsRequest request)
        {
            return new()
            {
                Rooms = await _roomRepository.GetRoomsByUserId(request.UserId)
            };
        }

        public async Task<GetRoomUserIdsResponse> GetRoomUserIds(GetRoomUserIdsRequest request)
        {
            return new()
            {
                UsersIds = await _userRoomRepository.GetRoomUsersIds(request.RoomId)
            };
        }

        public Task RemoveRoom(RemoveRoomRequest request)
        {
            return _roomRepository.Remove(request.RoomId);
        }
    }
}
