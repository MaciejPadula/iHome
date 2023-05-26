using iHome.Core.Models;
using System.Collections.Generic;

namespace iHome.Microservices.RoomsManagement.Contract.Models.Response
{
    public class GetRoomsResponse
    {
        public IEnumerable<RoomModel> Rooms { get; set; }
    }
}
