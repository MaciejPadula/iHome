using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iHome.Core.Models.Database
{
    public class TRoom
    {
        [Key]
        public int RoomId { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string UserId { get; set; } = "";

        public virtual List<TUserRoom> UsersRoom { get; set; } = new List<TUserRoom>();
        public virtual List<TDevice> Devices { get; set; } = new List<TDevice>();
    }
}
