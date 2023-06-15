using iHome.Microservices.UsersApi.Contract;
using iHome.Microservices.UsersApi.Contract.Models.Request;
using iHome.Microservices.UsersApi.Contract.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Microservices.UsersApi.Controllers
{
    public class UserManagementController : ControllerBase, IUserManagementService
    {
        private readonly IUserManagementService _userManagementService;

        public UserManagementController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        [HttpPost]
        public Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request)
        {
            return _userManagementService.GetUserById(request);
        }

        [HttpPost]
        public Task<GetUsersResponse> GetUsers(GetUsersRequest request)
        {
            return _userManagementService.GetUsers(request);
        }

        [HttpPost]
        public Task<UserExistResponse> UserExist(UserExistRequest request)
        {
            return _userManagementService.UserExist(request);
        }
    }
}
