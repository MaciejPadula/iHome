using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iHome.Core.Models.Database
{
    public class TDeviceToConfigure
    {
        [Key]
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public int DeviceType { get; set; }
        public string IpAddress { get; set; }

        public TDeviceToConfigure()
        {
        }

        public TDeviceToConfigure(string deviceId, int deviceType, string ipAddress)
        {
            DeviceId = deviceId;
            DeviceType = deviceType;
            IpAddress = ipAddress;
        }
    }
}
