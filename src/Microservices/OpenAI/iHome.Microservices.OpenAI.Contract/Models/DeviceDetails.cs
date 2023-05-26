using System;

namespace iHome.Microservices.OpenAI.Contract.Models
{
    public class DeviceDetails
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
