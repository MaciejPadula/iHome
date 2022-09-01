using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iHome.Models.Database
{
    public class TUsersRooms
    {
        [Key]
        public int Id { get; set; }
        public string Uuid { get; set; } = "";

        public int RoomId { get; set; }
    }
}
