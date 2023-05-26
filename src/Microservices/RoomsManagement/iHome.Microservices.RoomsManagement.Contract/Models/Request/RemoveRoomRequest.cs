using System;

namespace iHome.Microservices.RoomsManagement.Contract.Models.Request
{
    public class RemoveRoomRequest
    {
        public Guid RoomId { get; set; }
        public string UserId { get; set; }
    }
}
