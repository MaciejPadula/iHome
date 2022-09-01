using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iHome.Models.Database
{
    public class TRoom
    {
        [Key]
        public int RoomId { get; set; }
        public string RoomName { get; set; } = "";
        public string RoomImage { get; set; } = "";
        public string RoomDescription { get; set; } = "";
        public string Uuid { get; set; } = "";

        public virtual List<TDevice> Devices { get; set; } = new List<TDevice>();
    }
}
