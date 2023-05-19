using System;

namespace iHome.Microservices.RoomsManagement.Contract.Models.Request
{
    public class GetRoomUserIdsRequest
    {
        public Guid RoomId { get; set; }
        public string UserId { get; set; }
    }
}
