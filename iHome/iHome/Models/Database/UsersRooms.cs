using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iHome.Models.Database
{
    public class UsersRooms
    {
        [Key]
        public int Id { get; set; }
        public string? uuid { get; set; }

        public int roomId { get; set; }
    }
}
