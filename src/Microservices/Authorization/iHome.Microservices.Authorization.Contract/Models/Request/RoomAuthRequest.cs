using System;

namespace iHome.Microservices.Authorization.Contract.Models.Request
{
    public class RoomAuthRequest
    {
        public Guid RoomId { get; set; }
        public string UserId { get; set; }
    }
}
