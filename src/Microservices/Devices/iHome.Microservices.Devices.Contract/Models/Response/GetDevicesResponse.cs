using System.Collections.Generic;

namespace iHome.Microservices.Devices.Contract.Models.Response
{
    public class GetDevicesResponse
    {
        public IEnumerable<DeviceModel> Devices { get; set; }
    }
}
