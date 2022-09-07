using System.ComponentModel.DataAnnotations;

namespace iHome.Core.Models.Database
{
    public class TUsersRooms
    {
        [Key]
        public int Id { get; set; }
        public string Uuid { get; set; } = "";

        public int RoomId { get; set; }
    }
}
