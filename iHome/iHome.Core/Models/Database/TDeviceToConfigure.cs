using System.ComponentModel.DataAnnotations;

namespace iHome.Core.Models.Database
{
    public class TDeviceToConfigure
    {
        [Key]
        public Guid Id { get; set; }
        public string DeviceId { get; set; }
        public int DeviceType { get; set; }
        public string IpAddress { get; set; }

        public TDeviceToConfigure()
        {
            DeviceId = "";
            IpAddress = "";
        }

        public TDeviceToConfigure(Guid id, string deviceId, int deviceType, string ipAddress)
        {
            Id = id;
            DeviceId = deviceId;
            DeviceType = deviceType;
            IpAddress = ipAddress;
        }
    }
}
