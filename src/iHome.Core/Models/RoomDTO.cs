using iHome.Microservices.UsersApi.Contract.Models;

namespace iHome.Core.Models
{
    public class RoomDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public User User { get; set; }

        public RoomDTO(RoomModel room)
        {
            Id = room.Id;
            Name = room.Name;
            User = new User
            {
                Id = room.UserId
            };
        }
    }
}
