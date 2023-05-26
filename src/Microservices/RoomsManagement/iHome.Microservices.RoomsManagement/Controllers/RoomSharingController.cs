using iHome.Microservices.RoomsManagement.Contract;
using iHome.Microservices.RoomsManagement.Contract.Models.Request;
using iHome.Microservices.RoomsManagement.Contract.Models.Response;
using iHome.Microservices.RoomsManagement.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Microservices.RoomsManagement.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RoomSharingController : ControllerBase, IRoomSharingService
    {
        private readonly IUserRoomRepository _userRoomRepository;

        public RoomSharingController(IUserRoomRepository userRoomRepository)
        {
            _userRoomRepository = userRoomRepository;
        }

        [HttpPost]
        public async Task<GetRoomUserIdsResponse> GetRoomUserIds(GetRoomUserIdsRequest request)
        {
            //validation

            return new()
            {
                UsersIds = await _userRoomRepository.GetRoomUsersIds(request.RoomId)
            };
        }

        [HttpPost]
        public Task ShareRoomToUser(ShareRoomToUserRequest request)
        {
            //validation

            return _userRoomRepository.AddUserRoom(request.RoomId, request.SubjectUserId);
        }

        [HttpPost]
        public Task UnshareRoomFromUser(UnshareRoomFromUserRequest request)
        {
            //validation

            return _userRoomRepository.RemoveUserRoom(request.RoomId, request.SubjectUserId);
        }
    }
}
