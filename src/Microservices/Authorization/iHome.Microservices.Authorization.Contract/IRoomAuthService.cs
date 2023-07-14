using iHome.Microservices.Authorization.Contract.Models.Request;
using iHome.Microservices.Authorization.Contract.Models.Response;
using System.Threading.Tasks;

namespace iHome.Microservices.Authorization.Contract
{
    public interface IRoomAuthService
    {
        Task<AuthResponse> CanReadRoom(RoomAuthRequest request);
        Task<AuthResponse> CanWriteRoom(RoomAuthRequest request);
    }
}
