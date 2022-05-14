namespace iHome.Models.iHomeComponents
{
    public class Room
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public List<Device> devices { get; set; }
        public Room(int id, string name, string description, string image, List<Device> devices)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.image = image;
            this.devices = new List<Device>();
            devices.ForEach(device => this.devices.Add(device));
        }
    }
}
