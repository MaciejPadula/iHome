using iHome.Backend.Models.RoomsApi.Error;
using iHome.Core.Models.RoomsApi.Validators;

namespace iHome.Models.Requests
{
    public class RenameDeviceRequest
    {
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }

        public RenameDeviceRequest(string deviceId, string deviceName)
        {
            DeviceId = deviceId;
            DeviceName = deviceName;
        }

        public void Validate()
        {
            var validator = new RenameDeviceRequestValidator();
            if (!validator.Validate(this).IsValid)
                throw new RequestModelValidationException();
        }
    }
}
