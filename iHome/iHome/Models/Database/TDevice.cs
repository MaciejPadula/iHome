using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iHome.Models.Database
{
    public class TDevice
    {
        [Key]
        public string DeviceId { get; set; } = "";
        public string DeviceName { get; set; } = "";
        public int DeviceType { get; set; } = -1;
        public string DeviceData { get; set; } = "{}";

        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual TRoom Room { get; set; } = new TRoom();
    }
}
