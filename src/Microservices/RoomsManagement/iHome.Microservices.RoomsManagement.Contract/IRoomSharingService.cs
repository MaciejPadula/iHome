using iHome.Microservices.RoomsManagement.Contract.Models.Request;
using iHome.Microservices.RoomsManagement.Contract.Models.Response;
using System.Threading.Tasks;

namespace iHome.Microservices.RoomsManagement.Contract
{
    public interface IRoomSharingService
    {
        Task ShareRoomToUser(ShareRoomToUserRequest request);
        Task UnshareRoomFromUser(UnshareRoomFromUserRequest request);
        Task<GetRoomUserIdsResponse> GetRoomUserIds(GetRoomUserIdsRequest request);
    }
}
