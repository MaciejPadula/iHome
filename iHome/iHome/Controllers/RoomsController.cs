using iHome.Models;
using iHome.Models.iHomeComponents;
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
        [HttpGet("getrooms/{id}")]
        public ActionResult<Room> GetRooms(string id)
        {
            var rooms = database.GetRooms(id);// ("google-oauth2|115237564399157489610");
            if (rooms == null)
            {
                return NotFound();
            }
            return Ok(rooms);
        }
        [HttpPost("addroom")]
        public ActionResult<string> AddRoom([FromBody()] SimpleRoom room)
        {
            if (database.AddRoom(room.name, room.description, room.image, room.uuid))
                return Ok(room.uuid);
            return NotFound();
        }
        [HttpPost("removeroom/{id}")]
        public ActionResult<Room> RemoveRoom(int id)
        {
            if (database.RemoveRoom(id))
                return Ok();
            return NotFound();
        }
    }
}
