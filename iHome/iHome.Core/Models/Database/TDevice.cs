using iHome.Core.Models.ApiRooms;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iHome.Core.Models.Database
{
    public class TDevice
    {
        [Key]
        public string DeviceId { get; set; } = "";
        public string Name { get; set; } = "";
        public int Type { get; set; } = -1;
        public string Data { get; set; } = "{}";

        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual TRoom? Room { get; set; }

        public Device GetDevice()
        {
            return new Device()
            {
                Id = DeviceId,
                Name = Name,
                Type = Type,
                Data = Data,
                RoomId = RoomId
            };
        }
    }
}
