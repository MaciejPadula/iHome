using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iHome.Models.Database
{
    public class TDevice
    {
        [Key]
        public string deviceId { get; set; } = "";
        public string deviceName { get; set; } = "";
        public int deviceType { get; set; } = -1;
        public string deviceData { get; set; } = "{}";

        public int roomId { get; set; }
        [ForeignKey("roomId")]
        public virtual TRoom Room { get; set; } = new TRoom();
    }
}
