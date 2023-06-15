using iHome.Microservices.RoomsManagement.Contract;
using iHome.Microservices.RoomsManagement.Contract.Models.Request;
using iHome.Microservices.RoomsManagement.Contract.Models.Response;
using iHome.Microservices.RoomsManagement.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Microservices.RoomsManagement.Controllers
{
    public class RoomManagementController : ControllerBase, IRoomManagementService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomManagementController(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
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

        public Task RemoveRoom(RemoveRoomRequest request)
        {
            //validation

            return _roomRepository.Remove(request.RoomId);
        }
    }
}
