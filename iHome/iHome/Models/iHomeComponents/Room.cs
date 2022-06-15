namespace iHome.Models.iHomeComponents
{
    public class Room
    {
        public int roomId { get; set; }
        public string? roomName { get; set; }
        public string? roomDescription { get; set; }
        public string? roomImage { get; set; }
        public List<Device>? devices { get; set; }
    }
}
