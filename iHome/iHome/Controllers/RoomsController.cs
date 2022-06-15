using iHome.Models;
using iHome.Models.iHomeComponents;
using iHome.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace iHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private DatabaseModel database = new DatabaseModel("127.0.0.1", "root", "", "ihome");
            //"ihome.database.windows.net", "rootAdmin", "VcuraBEFKR6@3PX", "iHome");
        [HttpGet("GetRooms/")]
        [Authorize]
        public ActionResult<Room> GetRooms()
        {
            string? uuid = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            List<Room>? rooms = database.GetRooms(uuid);// ("google-oauth2|115237564399157489610");
            if (rooms == null)
            {
                return NotFound();
            }
            return Ok(rooms);
        }
        [HttpPost("AddRoom")]
        [Authorize]
        public ActionResult<string> AddRoom([FromBody()] SimpleRoom room)
        {
            string? uuid = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (database.AddRoom(room.roomName, room.roomDescription, room.roomImage, uuid))
            {
                return Ok(new AddRoomResponse { RoomName = room.roomName });
            }
            return NotFound();
        }
        [HttpPost("RemoveRoom/{id}")]
        [Authorize]
        public ActionResult<Room> RemoveRoom(int id)
        {
            if (database.RemoveRoom(id))
                return Ok(new RemoveRoomResponse{status=200});
            return NotFound();
        }
        [HttpGet("GetDevices/{roomId}")]
        [Authorize]
        public ActionResult<Room> GetDevices(int roomId)
        {
            var devices = database.GetDevices(roomId);// ("google-oauth2|115237564399157489610");
            if (devices == null)
            {
                return NotFound();
            }
            return Ok(devices);
        }
        [HttpPost("SetDeviceData")]
        [Authorize]
        public ActionResult<string> SetDeviceData([FromBody] DeviceData deviceData)
        {
            return Ok(database.SetDeviceData(deviceData.deviceId, deviceData.deviceData));
        }
        [HttpPost("SetDeviceRoom")]
        [Authorize]
        public ActionResult<string> SetDeviceData([FromBody] NewDeviceRoomData newDeviceRoomData)
        {
            if(database.UpdateDeviceRoom(newDeviceRoomData.deviceId, newDeviceRoomData.roomId))
                return Ok("Success");
            return NotFound();
        }
        [HttpGet("GetUserId")]
        [Authorize]
        public ActionResult<string> GetUserId()
        {
            return Ok(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
        }
    }
}
