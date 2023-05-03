namespace iHome.Models.Account.Rooms.Requests
{
    public class UserRoomRequest
    {
        public string Email { get; set; }
        public Guid RoomId { get; set; }

        public UserRoomRequest(string email, Guid roomId)
        {
            Email = email;
            RoomId = roomId;
        }
    }
}
