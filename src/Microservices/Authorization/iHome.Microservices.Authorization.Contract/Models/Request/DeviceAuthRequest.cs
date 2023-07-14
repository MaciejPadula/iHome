using System;

namespace iHome.Microservices.Authorization.Contract.Models.Request
{
    public class DeviceAuthRequest
    {
        public Guid DeviceId { get; set; }
        public string UserId { get; set; }
    }
}
