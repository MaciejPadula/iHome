namespace iHome.Core.Models.ApiRooms
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Device> Devices { get; set; }
        public string Uuid { get; set; }
        public string OwnerUuid { get; set; }

        public Room(int id, string name, string description, List<Device> devices, string uuid, string ownerUuid)
        {
            Id = id;
            Name = name;
            Description = description;
            Devices = devices;
            Uuid = uuid;
            OwnerUuid = ownerUuid;
        }
    }
}
