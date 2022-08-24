using iHome.Models.Database;

namespace iHome.Models.DataModels
{
    public class Room
    {
        public int roomId { get; set; } = 0;
        public string roomName { get; set; } = "";
        public string roomDescription { get; set; } = "";
        public string roomImage { get; set; } = "";
        public List<Device> devices { get; set; } = new List<Device>();
        public string uuid { get; set; } = "";
        public string masterUuid { get; set; } = "";
    }
}
