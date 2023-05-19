using iHome.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using iHome.Microservices.RoomsManagement.Contract.Models.Request;
using iHome.Microservices.RoomsManagement.Contract.Models.Response;

namespace iHome.Microservices.RoomsManagement.Contract
{
    public interface IRoomManagementService
    {
        Task AddRoom(AddRoomRequest request);
        Task<GetRoomsResponse> GetRooms(GetRoomsRequest request);
        Task RemoveRoom(RemoveRoomRequest request);
        Task<GetRoomUserIdsResponse> GetRoomUserIds(GetRoomUserIdsRequest request);
    }
}
