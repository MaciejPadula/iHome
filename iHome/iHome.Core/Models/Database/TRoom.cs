using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iHome.Core.Models.Database
{
    public class TRoom
    {
        [Key]
        public int RoomId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }

        public virtual List<TUserRoom> UsersRoom { get; set; }
        public virtual List<TDevice> Devices { get; set; }

        public TRoom()
        {
        }

        public TRoom(string name, string description, string userId, List<TUserRoom> usersRoom, List<TDevice> devices)
        {
            Name = name;
            Description = description;
            UserId = userId;
            UsersRoom = usersRoom;
            Devices = devices;
        }
    }
}
