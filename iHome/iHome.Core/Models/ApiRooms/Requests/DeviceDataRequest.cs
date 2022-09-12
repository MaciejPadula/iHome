namespace iHome.Core.Models.Requests
{
    public class DeviceDataRequest
    {
        public string DeviceId { get; set; }
        public string DeviceData { get; set; }

        public DeviceDataRequest(string deviceId, string deviceData)
        {
            DeviceId = deviceId;
            DeviceData = deviceData;
        }
    }
}
