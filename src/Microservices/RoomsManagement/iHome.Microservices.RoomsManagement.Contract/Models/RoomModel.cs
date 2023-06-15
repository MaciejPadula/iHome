using System;

namespace iHome.Microservices.RoomsManagement.Contract.Models
{
    public class RoomModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
    }
}