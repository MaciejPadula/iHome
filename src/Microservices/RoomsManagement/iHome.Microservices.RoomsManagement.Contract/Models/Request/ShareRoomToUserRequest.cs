using System;

namespace iHome.Microservices.RoomsManagement.Contract.Models.Request
{
    public class ShareRoomToUserRequest
    {
        public Guid RoomId { get; set; }
        public string CallerUserId { get; set; }
        public string SubjectUserId { get; set; }
    }
}
