using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iHome.Models.Database
{
    public class TRoom
    {
        [Key]
        public int roomId { get; set; }
        public string? roomName { get; set; }
        public string? roomImage { get; set; }
        public string? roomDescription { get; set; }
        public string? uuid { get; set; }

        public virtual List<TDevice> devices { get; set; }
    }
}
