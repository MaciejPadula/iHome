using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iHome.Core.Models.Database
{
    public class TUserRoom
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual TRoom Room { get; set; }

        public TUserRoom()
        {
            UserId = "";
            Room = new TRoom();
        }

        public TUserRoom(Guid id, string userId, Guid roomId, TRoom room)
        {
            Id = id;
            UserId = userId;
            RoomId = roomId;
            Room = room;
        }
    }
}
