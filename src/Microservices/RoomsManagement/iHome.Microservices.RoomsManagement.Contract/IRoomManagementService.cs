using iHome.Microservices.RoomsManagement.Contract.Models.Request;
using iHome.Microservices.RoomsManagement.Contract.Models.Response;
using System.Threading.Tasks;

namespace iHome.Microservices.RoomsManagement.Contract
{
    public interface IRoomManagementService
    {
        Task AddRoom(AddRoomRequest request);
        Task<GetRoomsResponse> GetRooms(GetRoomsRequest request);
        Task RemoveRoom(RemoveRoomRequest request);
    }
}
