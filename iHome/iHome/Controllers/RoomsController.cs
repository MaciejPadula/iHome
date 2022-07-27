using iHome.Models;
using iHome.Models.Account;
using iHome.Models.Account.Rooms.Requests;
using iHome.Models.Database;
using iHome.Models.DataModels;
using iHome.Models.Requests;
using iHome.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Security.Claims;

namespace iHome.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly DatabaseSettings config;
        private IDatabaseApi databaseApi;
        private UserInfo userInfo;
        public RoomsController()
        {
            userInfo = new UserInfo();
            config = new ConfigurationLoader().loadDatabaseSettings("appsettings.json");
            databaseApi = new AzureDatabaseApi(
                config.DatabaseServer,
                config.DatabaseLogin,
                config.DatabasePassword,
                config.DatabaseName
            );
        }

        [HttpGet("GetRooms/")]
        [Authorize]
        public ActionResult GetRooms()
        {
            string? uuid = userInfo.GetUserUuid(User);
            if(uuid == null) return NotFound();

            var listOfRooms = databaseApi.GetListOfRooms(uuid);

            if (listOfRooms == null)
            {
                return NotFound();
            }
            return Ok(listOfRooms);
        }

        [HttpPost("AddRoom/")]
        [Authorize]
        public ActionResult AddRoom([FromBody()] RoomRequest room)
        {
            string? uuid = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if(room==null || uuid==null) return NotFound(new { exception = "room or uuid is null" });
            if (databaseApi.AddRoom(room.roomName, room.roomDescription, uuid))
            {
                return Ok(new { status = 200 });
            }
            return NotFound(new { exception = "Can't add new room" });
        }

        [HttpPost("RemoveRoom/{id}")]
        [Authorize]
        public ActionResult RemoveRoom(int id)
        {
            if (databaseApi.RemoveRoom(id))
            {
                return Ok(new { status = 200 });
            }
            return NotFound(new { exception = "Can't remove room" });
        }

        [HttpPost("ShareRoom")]
        [Authorize]
        public ActionResult ShareRoom([FromBody] UserRoomRequest userRoom)
        {
            string? uuid = userInfo.GetUserUuid(userRoom.email);
            if (uuid == null) return NotFound(new { exception = "Uuid equals null" });
            if (databaseApi.ShareRoom(userRoom.roomId, uuid))
            {
                return Ok(new { status = 200 });
            }
            return NotFound(new { exception = "Can't share room" });
        }

        [HttpGet("GetRoomsCount")]
        [Authorize]
        public ActionResult GetRoomsCount()
        {
            string? uuid = userInfo.GetUserUuid(User);
            if(uuid == null) return NotFound();
            int roomsCount = databaseApi.GetRoomsCount(uuid);
            return Ok(new { roomsCount });
        }

        [HttpGet("GetDevices/{roomId}")]
        [Authorize]
        public ActionResult GetDevices(int roomId)
        {
            //var devices = database.GetDevices(roomId);// ("google-oauth2|115237564399157489610");
            var devices = databaseApi.GetDevices(roomId);
            if (devices != null)
            {
                return Ok(devices);
            }
            return NotFound(new { exception = "Can't get rooms" });
        }

        [HttpPost("AddDevice/{id}")]
        [Authorize]
        public ActionResult AddDevice(int id, [FromBody] Device device)
        {
            if(device == null) return NotFound(new { exception = "Wrong input data" });

            if (databaseApi.AddDevice(id, device.deviceId, device.deviceName, device.deviceType, device.deviceData, device.roomId))
            {
                return Ok(new { status = 200 });
            }
            return NotFound(new { exception = "Can't add device" });
        }

        [HttpPost("RenameDevice/")]
        [Authorize]
        public ActionResult RenameDevice([FromBody] RenameDeviceRequest renameDevice)
        {
            if(renameDevice == null) return NotFound(new { exception = "Wrong input data" });
            if (databaseApi.RenameDevice(renameDevice.deviceId, renameDevice.deviceName))
            {
                return Ok(new { status = 200 });
            }
            return NotFound(new { exception = "Can't rename device" });
        }

        [HttpGet("GetDevicesCount")]
        [Authorize]
        public ActionResult GetDevicesCount()
        {
            string? uuid = userInfo.GetUserUuid(User);
            if (uuid == null) return NotFound(new { exception = "Uuid equals null" });
            var devicesCount = databaseApi.GetDevicesCount(uuid);
            return Ok(new { devicesCount });
        }

        [HttpGet("GetDeviceData/{deviceId}")]
        public ActionResult GetDeviceData(string deviceId)
        {
            var deviceData = databaseApi.GetDeviceData(deviceId);
            if (deviceData != null)
            {
                return Ok(deviceData);
            }
            return NotFound(new { exception = "Can't read device data" });
        }
        [HttpPost("SetDeviceData/")]
        [Authorize]
        public ActionResult SetDeviceData([FromBody] DeviceDataRequest deviceData)
        {
            if(databaseApi.SetDeviceData(deviceData.deviceId, deviceData.deviceData))
            {
                return Ok(new { status = 200 });
            }
            return NotFound(new { exception = "Can't set device data" });
        }

        [HttpPost("SetDeviceRoom/")]
        [Authorize]
        public ActionResult SetDeviceRoom([FromBody] NewDeviceRoomRequest newDeviceRoom)
        {
            if (databaseApi.SetDeviceRoom(newDeviceRoom.deviceId, newDeviceRoom.roomId))
            {
                return Ok(new { status = 200 });
            }
            return NotFound(new { exception = "Can't change device room" });
        }
        
        [HttpGet("GetUserId/")]
        [Authorize]
        public ActionResult GetUserId()
        {
            return Ok(userInfo.GetUserUuid(User));
        }

        [HttpGet("GetDevicesToConfigure/")]
        [Authorize]
        public async Task<ActionResult> GetDevicesToConfigure()
        {
            var ip = await userInfo.GetPublicIp(HttpContext);
            if(ip==null) return NotFound(new { exception = "IP equals null" });
            var devicesToConfigure = databaseApi.GetDevicesToConfigure(ip);
            if (devicesToConfigure != null)
            {
                return Ok(devicesToConfigure);
            }
            return NotFound(new { exception = "Can't get devices to configure list" });
        }

        [HttpPost("AddDevicesToConfigure/")]
        [Authorize]
        public async Task<ActionResult> AddDevicesToConfigure([FromBody] NewDeviceToConfigureRequest deviceToConfigure)
        {
            var ip = await userInfo.GetPublicIp(HttpContext);
            if(ip==null || deviceToConfigure == null) return NotFound(new { exception = "IP or input data equals null" });
            if (databaseApi.AddDevicesToConfigure(deviceToConfigure.deviceId, deviceToConfigure.deviceType, ip))
            {
                return Ok(new { status = 200 });
            }
            return NotFound(new { exception = "Can't add new device to configure" });

        }
    }
}
