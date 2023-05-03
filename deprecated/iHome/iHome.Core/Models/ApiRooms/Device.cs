using iHome.Core.Middleware.Exceptions;
using iHome.Core.Models.RoomsApi.Validators;

namespace iHome.Core.Models.ApiRooms
{
    public class Device
    {
        public string Id { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Data { get; set; } = "{}";
        public Guid RoomId { get; set; }

        public Device(string id, int type, string name, Guid roomId)
        {
            Id = id;
            Type = type;
            Name = name;
            RoomId = roomId;
        }

        public void Validate()
        {
            var validator = new DeviceRequestValidator();
            if (!validator.Validate(this).IsValid)
                throw new RequestModelValidationException();
        }
    }
}
