using iHome.Core.Middleware.Exceptions;
using iHome.Core.Models.RoomsApi.Validators;

namespace iHome.Models.Requests
{
    public class RoomRequest
    {
        public string RoomName { get; set; }
        public string RoomDescription { get; set; }

        public RoomRequest(string roomName, string roomDescription)
        {
            RoomName = roomName;
            RoomDescription = roomDescription;
        }

        public void Validate()
        {
            var validationRules = new RoomRequestValidator();
            if (!validationRules.Validate(this).IsValid)
                throw new RequestModelValidationException();
        }
    }
}
