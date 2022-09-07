namespace iHome.Core.Models.ApiRooms
{
    public class Device
    {
        public string Id { get; set; } = "";
        public int Type { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Data { get; set; } = "";
        public int RoomId { get; set; } = 0;
    }
}
