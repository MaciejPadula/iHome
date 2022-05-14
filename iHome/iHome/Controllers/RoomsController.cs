using iHome.Models;
using iHome.Models.iHomeComponents;
using iHome.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace iHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private DatabaseModel database = new DatabaseModel("ihome.database.windows.net", "rootAdmin", "VcuraBEFKR6@3PX", "iHome");
        [HttpGet("GetRooms/{userId}")]
        public ActionResult<Room> GetRooms(string userId)
        {
            var rooms = database.GetRooms(userId);// ("google-oauth2|115237564399157489610");
            if (rooms == null)
            {
                return NotFound();
            }
            return Ok(rooms);
        }
        [HttpPost("AddRoom")]
        public ActionResult<string> AddRoom([FromBody()] SimpleRoom room)
        {
            if (database.AddRoom(room.name, room.description, room.image, room.uuid))
                return Ok(new AddRoomResponse{ RoomName = room.name });
            return NotFound();
        }
        [HttpPost("RemoveRoom/{id}")]
        public ActionResult<Room> RemoveRoom(int id)
        {
            if (database.RemoveRoom(id))
                return Ok(new RemoveRoomResponse{status=200});
            return NotFound();
        }
        [HttpGet("GetDevices/{roomId}")]
        public ActionResult<Room> GetRooms(int roomId)
        {
            var devices = database.GetDevices(roomId);// ("google-oauth2|115237564399157489610");
            if (devices == null)
            {
                return NotFound();
            }
            return Ok(devices);
        }
    }
}
