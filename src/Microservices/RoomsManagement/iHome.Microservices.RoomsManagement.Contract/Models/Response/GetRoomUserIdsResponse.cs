using System.Collections.Generic;

namespace iHome.Microservices.RoomsManagement.Contract.Models.Response
{
    public class GetRoomUserIdsResponse
    {
        public IEnumerable<string> UsersIds { get; set; }
    }
}
