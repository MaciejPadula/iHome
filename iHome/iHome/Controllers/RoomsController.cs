using iHome.Models;
using iHome.Models.Database;
using iHome.Models.DataModels;
using iHome.Models.Requests;
using iHome.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace iHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private ApplicationdbContext db = new ApplicationdbContext(
            new ConnectionStringBuilder("ihomedevice.database.windows.net")
            .withLogin("tritIouR")
            .withPassword("88swNNgWXt2jr5F")
            .withInitialCatalog("ihomedevice")
            .build()
        );
        [HttpGet("GetRooms/")]
        [Authorize]
        public ActionResult GetRooms()
        {
            string? uuid = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var listOfRooms = db.Rooms
                .Include(room => room.devices)
                .Where(room => room.uuid == uuid)
                .Select(x => DataModelsConversionUtils.RoomFromTRoom(x))
                .ToList();
            if (listOfRooms.Count > 0)
            {
                return Ok(listOfRooms);
            }
            return NotFound(new HTTPResponse { status = 404 });
        }

        [HttpPost("AddRoom/")]
        [Authorize]
        public ActionResult AddRoom([FromBody()] RoomRequest room)
        {
            string? uuid = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            db.Rooms.Add(new TRoom()
            { 
                roomName = room.roomName,
                roomDescription = room.roomDescription,
                roomImage = room.roomImage,
                uuid = uuid
            });
            if(db.SaveChanges() > 0)
            {
                return Ok(new HTTPResponse { status = 200 });
            }
            return NotFound(new HTTPResponse { status = 404 });
        }

        [HttpPost("RemoveRoom/{id}")]
        [Authorize]
        public ActionResult RemoveRoom(int id)
        {
            string? uuid = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var roomToRemove = db.Rooms.Where(room => room.roomId == id).FirstOrDefault();
            if (roomToRemove != null)
            {
                db.Rooms.Remove(roomToRemove);
                db.SaveChanges();
                return Ok(new HTTPResponse { status = 200 });
            }
            return NotFound(new HTTPResponse { status = 404 });
        }
        [HttpGet("GetDevices/{roomId}")]
        [Authorize]
        public ActionResult GetDevices(int roomId)
        {
            //var devices = database.GetDevices(roomId);// ("google-oauth2|115237564399157489610");
            var devices = db.Devices.Where(device => device.roomId == roomId).ToList();
            if (devices != null)
            {
                return Ok(DataModelsConversionUtils.ListOfDevicesFromListOfTDevices(devices));
            }
            return NotFound(new HTTPResponse { status = 404 });
        }
        [HttpPost("GetDeviceData/{deviceId}")]
        public ActionResult GetDeviceData(string deviceId)
        {
            var deviceData = db.Devices
                    .Where(device => device.deviceId == deviceId)
                    .Select(device => device.deviceData)
                    .FirstOrDefault();
            if (deviceData != null)
            {
                return Ok(deviceData);
            }
            return NotFound(new HTTPResponse{ status = 404 });
        }
        [HttpPost("SetDeviceData/")]
        [Authorize]
        public ActionResult SetDeviceData([FromBody] DeviceDataRequest deviceData)
        {
            var device = db.Devices.FirstOrDefault(device => device.deviceId == deviceData.deviceId);
            if (device != null)
            {
                device.deviceData = deviceData.deviceData;
                db.Entry(device).State = EntityState.Modified;
                db.SaveChanges();
                return Ok(new HTTPResponse { status = 200 });
            }
            return NotFound(new HTTPResponse { status = 404 });
        }
        [HttpPost("SetDeviceRoom/")]
        [Authorize]
        public ActionResult SetDeviceRoom([FromBody] NewDeviceRoomRequest newDeviceRoomData)
        {
            var deviceToChange = db.Devices.Where(device => device.deviceId == newDeviceRoomData.deviceId).FirstOrDefault();
            if (deviceToChange != null)
            {
                deviceToChange.roomId = newDeviceRoomData.roomId;
                db.Entry(deviceToChange).State = EntityState.Modified;
                db.SaveChanges();
                return Ok(new HTTPResponse { status = 200 });
            } 
            return NotFound(new HTTPResponse { status = 404 });
        }
        [HttpPost("RenameDevice/")]
        [Authorize]
        public ActionResult RenameDevice([FromBody] RenameDeviceRequest renameDeviceRequest)
        {
            var deviceToChange = db.Devices.Where(device => device.deviceId == renameDeviceRequest.deviceId).FirstOrDefault();
            if (deviceToChange != null)
            {
                deviceToChange.deviceName = renameDeviceRequest.deviceName;
                db.Entry(deviceToChange).State = EntityState.Modified;
                db.SaveChanges();
                return Ok(new HTTPResponse { status = 200 });
            }
            return NotFound(new HTTPResponse { status = 404 });
        }
        [HttpGet("GetUserId/")]
        [Authorize]
        public ActionResult GetUserId()
        {
            return Ok(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
        }
    }
}
