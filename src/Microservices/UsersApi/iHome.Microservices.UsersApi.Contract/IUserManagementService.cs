using iHome.Microservices.UsersApi.Contract.Models.Request;
using iHome.Microservices.UsersApi.Contract.Models.Response;
using System.Threading.Tasks;

namespace iHome.Microservices.UsersApi.Contract
{
    public interface IUserManagementService
    {
        Task<GetUsersResponse> GetUsers(GetUsersRequest request);
        Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request);
        Task<UserExistResponse> UserExist(UserExistRequest request);
        Task<GetUsersByIdsResponse> GetUsersByIds(GetUsersByIdsRequest request);
    }
}
