using iHome.Core.Models.ApiRooms;
using iHome.Core.Models.Database;

namespace iHome.Core.Helpers
{
    public static class ListExtensions
    {
        public static List<Device> GetDeviceList(this List<TDevice> devices)
        {
            if(devices == null)
            {
                return new();
            }

            var devicesList = new List<Device>();
            devices.ForEach(d => devicesList.Add(d.GetDevice()));
            return devicesList;
        }
    }
}
