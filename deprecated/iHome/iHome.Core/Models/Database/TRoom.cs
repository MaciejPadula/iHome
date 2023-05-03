using System.ComponentModel.DataAnnotations;

namespace iHome.Core.Models.Database
{
    public class TRoom
    {
        [Key]
        public Guid RoomId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }

        public virtual List<TUserRoom> UsersRoom { get; set; }
        public virtual List<TDevice> Devices { get; set; }

        public TRoom()
        {
            Name = "";
            Description = "";
            UserId = "";
            UsersRoom = new List<TUserRoom>();
            Devices = new List<TDevice>();
        }

        public TRoom(Guid roomId, string name, string description, string userId, List<TUserRoom> usersRoom, List<TDevice> devices)
        {
            RoomId = roomId;
            Name = name;
            Description = description;
            UserId = userId;
            UsersRoom = usersRoom;
            Devices = devices;
        }
    }
}
