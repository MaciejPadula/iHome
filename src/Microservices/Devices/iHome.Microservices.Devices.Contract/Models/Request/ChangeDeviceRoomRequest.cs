using System;

namespace iHome.Microservices.Devices.Contract.Models.Request
{
    public class ChangeDeviceRoomRequest
    {
        public Guid DeviceId { get; set; }
        public Guid RoomId { get; set; }
    }
}
