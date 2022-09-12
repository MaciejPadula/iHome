namespace iHome.Models.Account.Rooms.Requests
{
    public class UserRoomRequest
    {
        public string Email { get; set; }
        public int RoomId { get; set; }

        public UserRoomRequest(string email, int roomId)
        {
            Email = email;
            RoomId = roomId;
        }
    }
}
