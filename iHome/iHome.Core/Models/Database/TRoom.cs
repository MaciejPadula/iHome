using System.ComponentModel.DataAnnotations;

namespace iHome.Core.Models.Database
{
    public class TRoom
    {
        [Key]
        public int RoomId { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string UserId { get; set; } = "";

        public virtual List<TDevice> Devices { get; set; } = new List<TDevice>();
    }
}
