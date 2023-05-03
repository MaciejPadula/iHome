using iHome.Core.Models.ApiRooms;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iHome.Core.Models.Database
{
    public class TDevice
    {
        [Key]
        public string DeviceId { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string Data { get; set; }

        public Guid RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual TRoom Room { get; set; }

        public TDevice()
        {
            DeviceId = "";
            Name = "";
            Data = "{}";
            Room = new TRoom();
        }

        public TDevice(string deviceId, string name, int type, string data, Guid roomId, TRoom room)
        {
            DeviceId = deviceId;
            Name = name;
            Type = type;
            Data = data;
            RoomId = roomId;
            Room = room;
        }

        public Device GetDevice()
        {
            return new Device(DeviceId, Type, Name, RoomId)
            {
                Data = Data
            };
        }
    }
}
