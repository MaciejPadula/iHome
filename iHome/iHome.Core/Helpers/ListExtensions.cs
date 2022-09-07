using iHome.Core.Models.ApiRooms;
using iHome.Core.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHome.Core.Helpers
{
    public static class ListExtensions
    {
        public static List<Device> GetDeviceList(this List<TDevice> devices)
        {
            var devicesList = new List<Device>();
            devices.ForEach(d => devicesList.Add(d.GetDevice()));
            return devicesList;
        }
    }
}
