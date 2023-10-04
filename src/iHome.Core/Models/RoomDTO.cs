using iHome.Microservices.RoomsManagement.Contract.Models;
using iHome.Microservices.UsersApi.Contract.Models;

namespace iHome.Core.Models
{
    public class RoomDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public User? User { get; set; }

        public RoomDto()
        {}

        public RoomDto(RoomModel room, User user)
        {
            Id = room.Id;
            Name = room.Name;
            User = user;
        }
    }
}
