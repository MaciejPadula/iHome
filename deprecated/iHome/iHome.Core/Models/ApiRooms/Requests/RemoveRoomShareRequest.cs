namespace iHome.Models.RoomsApi.Requests
{
    public class RemoveRoomShareRequest
    {
        public Guid RoomId { get; set; }
        public string Uuid { get; set; }

        public RemoveRoomShareRequest(Guid roomId, string uuid)
        {
            RoomId = roomId;
            Uuid = uuid;
        }
    }
}
