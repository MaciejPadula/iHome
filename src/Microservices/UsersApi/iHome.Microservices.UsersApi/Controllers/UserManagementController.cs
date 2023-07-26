using iHome.Microservices.UsersApi.Contract;
using iHome.Microservices.UsersApi.Contract.Models.Request;
using iHome.Microservices.UsersApi.Contract.Models.Response;
using iHome.Microservices.UsersApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Microservices.UsersApi.Controllers
{
    public class UserManagementController : ControllerBase, IUserManagementService
    {
        private readonly IUserService _userService;

        public UserManagementController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public Task<GetUserByIdResponse> GetUserById([FromBody] GetUserByIdRequest request)
        {
            return _userService.GetUserById(request);
        }

        [HttpPost]
        public Task<GetUsersResponse> GetUsers([FromBody] GetUsersRequest request)
        {
            return _userService.GetFilteredUsers(request);
        }

        [HttpPost]
        public Task<GetUsersByIdsResponse> GetUsersByIds([FromBody] GetUsersByIdsRequest request)
        {
            return _userService.GetUsersByIds(request);
        }

        [HttpPost]
        public Task<UserExistResponse> UserExist([FromBody] UserExistRequest request)
        {
            return _userService.UserExist(request);
        }
    }
}
