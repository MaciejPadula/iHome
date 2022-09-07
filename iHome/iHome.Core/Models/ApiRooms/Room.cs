namespace iHome.Core.Models.ApiRooms
{
    public class Room
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Image { get; set; } = "";
        public List<Device> Devices { get; set; } = new List<Device>();
        public string Uuid { get; set; } = "";
        public string OwnerUuid { get; set; } = "";
    }
}
