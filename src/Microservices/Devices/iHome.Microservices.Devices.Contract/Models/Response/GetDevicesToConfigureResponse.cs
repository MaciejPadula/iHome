using System.Collections.Generic;

namespace iHome.Microservices.Devices.Contract.Models.Response
{
    public class GetDevicesToConfigureResponse
    {
        public IEnumerable<DeviceToConfigure> Devices { get; set; }
    }
}
