namespace iHome.Models.RoomsApi.Requests
{
    public class RemoveRoomShareRequest
    {
        public int RoomId { get; set; }
        public string Uuid { get; set; }

        public RemoveRoomShareRequest(int roomId, string uuid)
        {
            RoomId = roomId;
            Uuid = uuid;
        }
    }
}
