using iHome.Models;
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
        private ApplicationdbContext db;
        public RoomsController()
        {
            config = new ConfigurationLoader().loadDatabaseSettings("appsettings.json");
            db = new ApplicationdbContext(
            new ConnectionStringBuilder(config.DatabaseServer)
                .withLogin(config.DatabaseLogin)
                .withPassword(config.DatabasePassword)
                .withInitialCatalog(config.DatabaseName)
                .build()
            );
        }

        private List<Room> getUserListOfRooms(string uuid)
        {
            return db.Rooms
                    .Include(room => room.devices)
                    .Join(db.UsersRooms,
                        room => room.roomId,
                        userRoom => userRoom.roomId,
                        (room, userRoom) => new Room
                        {
                            roomId = room.roomId,
                            roomDescription = room.roomDescription,
                            roomImage = room.roomImage,
                            roomName = room.roomName,
                            devices = DataModelsConversionUtils.ListOfDevicesFromListOfTDevices(room.devices),
                            uuid = userRoom.uuid
                        }
                    ).Where(room => room.uuid == uuid).ToList();
        }
        private string GetUUIDFromEmail(string email)
        {
            try
            {
                var client = new RestClient("https://dev-e7eyj4xg.eu.auth0.com/api/v2/users?q=email:" + email + "&search_engine=v3");
                var request = new RestRequest();
                request.AddHeader("authorization", "Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IkxKdHR5NlBhNnNHZ3lMb0dzX2pYcCJ9.eyJpc3MiOiJodHRwczovL2Rldi1lN2V5ajR4Zy5ldS5hdXRoMC5jb20vIiwic3ViIjoiazNTYjhtNjRqSFNPRXJ1QkVNZVZEWWs1TlZPTENjdU9AY2xpZW50cyIsImF1ZCI6Imh0dHBzOi8vZGV2LWU3ZXlqNHhnLmV1LmF1dGgwLmNvbS9hcGkvdjIvIiwiaWF0IjoxNjU3NTU0MDc4LCJleHAiOjE2NTc2NDA0NzgsImF6cCI6ImszU2I4bTY0akhTT0VydUJFTWVWRFlrNU5WT0xDY3VPIiwic2NvcGUiOiJyZWFkOmNsaWVudF9ncmFudHMgY3JlYXRlOmNsaWVudF9ncmFudHMgZGVsZXRlOmNsaWVudF9ncmFudHMgdXBkYXRlOmNsaWVudF9ncmFudHMgcmVhZDp1c2VycyB1cGRhdGU6dXNlcnMgZGVsZXRlOnVzZXJzIGNyZWF0ZTp1c2VycyByZWFkOnVzZXJzX2FwcF9tZXRhZGF0YSB1cGRhdGU6dXNlcnNfYXBwX21ldGFkYXRhIGRlbGV0ZTp1c2Vyc19hcHBfbWV0YWRhdGEgY3JlYXRlOnVzZXJzX2FwcF9tZXRhZGF0YSByZWFkOnVzZXJfY3VzdG9tX2Jsb2NrcyBjcmVhdGU6dXNlcl9jdXN0b21fYmxvY2tzIGRlbGV0ZTp1c2VyX2N1c3RvbV9ibG9ja3MgY3JlYXRlOnVzZXJfdGlja2V0cyByZWFkOmNsaWVudHMgdXBkYXRlOmNsaWVudHMgZGVsZXRlOmNsaWVudHMgY3JlYXRlOmNsaWVudHMgcmVhZDpjbGllbnRfa2V5cyB1cGRhdGU6Y2xpZW50X2tleXMgZGVsZXRlOmNsaWVudF9rZXlzIGNyZWF0ZTpjbGllbnRfa2V5cyByZWFkOmNvbm5lY3Rpb25zIHVwZGF0ZTpjb25uZWN0aW9ucyBkZWxldGU6Y29ubmVjdGlvbnMgY3JlYXRlOmNvbm5lY3Rpb25zIHJlYWQ6cmVzb3VyY2Vfc2VydmVycyB1cGRhdGU6cmVzb3VyY2Vfc2VydmVycyBkZWxldGU6cmVzb3VyY2Vfc2VydmVycyBjcmVhdGU6cmVzb3VyY2Vfc2VydmVycyByZWFkOmRldmljZV9jcmVkZW50aWFscyB1cGRhdGU6ZGV2aWNlX2NyZWRlbnRpYWxzIGRlbGV0ZTpkZXZpY2VfY3JlZGVudGlhbHMgY3JlYXRlOmRldmljZV9jcmVkZW50aWFscyByZWFkOnJ1bGVzIHVwZGF0ZTpydWxlcyBkZWxldGU6cnVsZXMgY3JlYXRlOnJ1bGVzIHJlYWQ6cnVsZXNfY29uZmlncyB1cGRhdGU6cnVsZXNfY29uZmlncyBkZWxldGU6cnVsZXNfY29uZmlncyByZWFkOmhvb2tzIHVwZGF0ZTpob29rcyBkZWxldGU6aG9va3MgY3JlYXRlOmhvb2tzIHJlYWQ6YWN0aW9ucyB1cGRhdGU6YWN0aW9ucyBkZWxldGU6YWN0aW9ucyBjcmVhdGU6YWN0aW9ucyByZWFkOmVtYWlsX3Byb3ZpZGVyIHVwZGF0ZTplbWFpbF9wcm92aWRlciBkZWxldGU6ZW1haWxfcHJvdmlkZXIgY3JlYXRlOmVtYWlsX3Byb3ZpZGVyIGJsYWNrbGlzdDp0b2tlbnMgcmVhZDpzdGF0cyByZWFkOmluc2lnaHRzIHJlYWQ6dGVuYW50X3NldHRpbmdzIHVwZGF0ZTp0ZW5hbnRfc2V0dGluZ3MgcmVhZDpsb2dzIHJlYWQ6bG9nc191c2VycyByZWFkOnNoaWVsZHMgY3JlYXRlOnNoaWVsZHMgdXBkYXRlOnNoaWVsZHMgZGVsZXRlOnNoaWVsZHMgcmVhZDphbm9tYWx5X2Jsb2NrcyBkZWxldGU6YW5vbWFseV9ibG9ja3MgdXBkYXRlOnRyaWdnZXJzIHJlYWQ6dHJpZ2dlcnMgcmVhZDpncmFudHMgZGVsZXRlOmdyYW50cyByZWFkOmd1YXJkaWFuX2ZhY3RvcnMgdXBkYXRlOmd1YXJkaWFuX2ZhY3RvcnMgcmVhZDpndWFyZGlhbl9lbnJvbGxtZW50cyBkZWxldGU6Z3VhcmRpYW5fZW5yb2xsbWVudHMgY3JlYXRlOmd1YXJkaWFuX2Vucm9sbG1lbnRfdGlja2V0cyByZWFkOnVzZXJfaWRwX3Rva2VucyBjcmVhdGU6cGFzc3dvcmRzX2NoZWNraW5nX2pvYiBkZWxldGU6cGFzc3dvcmRzX2NoZWNraW5nX2pvYiByZWFkOmN1c3RvbV9kb21haW5zIGRlbGV0ZTpjdXN0b21fZG9tYWlucyBjcmVhdGU6Y3VzdG9tX2RvbWFpbnMgdXBkYXRlOmN1c3RvbV9kb21haW5zIHJlYWQ6ZW1haWxfdGVtcGxhdGVzIGNyZWF0ZTplbWFpbF90ZW1wbGF0ZXMgdXBkYXRlOmVtYWlsX3RlbXBsYXRlcyByZWFkOm1mYV9wb2xpY2llcyB1cGRhdGU6bWZhX3BvbGljaWVzIHJlYWQ6cm9sZXMgY3JlYXRlOnJvbGVzIGRlbGV0ZTpyb2xlcyB1cGRhdGU6cm9sZXMgcmVhZDpwcm9tcHRzIHVwZGF0ZTpwcm9tcHRzIHJlYWQ6YnJhbmRpbmcgdXBkYXRlOmJyYW5kaW5nIGRlbGV0ZTpicmFuZGluZyByZWFkOmxvZ19zdHJlYW1zIGNyZWF0ZTpsb2dfc3RyZWFtcyBkZWxldGU6bG9nX3N0cmVhbXMgdXBkYXRlOmxvZ19zdHJlYW1zIGNyZWF0ZTpzaWduaW5nX2tleXMgcmVhZDpzaWduaW5nX2tleXMgdXBkYXRlOnNpZ25pbmdfa2V5cyByZWFkOmxpbWl0cyB1cGRhdGU6bGltaXRzIGNyZWF0ZTpyb2xlX21lbWJlcnMgcmVhZDpyb2xlX21lbWJlcnMgZGVsZXRlOnJvbGVfbWVtYmVycyByZWFkOmVudGl0bGVtZW50cyByZWFkOmF0dGFja19wcm90ZWN0aW9uIHVwZGF0ZTphdHRhY2tfcHJvdGVjdGlvbiByZWFkOm9yZ2FuaXphdGlvbnNfc3VtbWFyeSByZWFkOm9yZ2FuaXphdGlvbnMgdXBkYXRlOm9yZ2FuaXphdGlvbnMgY3JlYXRlOm9yZ2FuaXphdGlvbnMgZGVsZXRlOm9yZ2FuaXphdGlvbnMgY3JlYXRlOm9yZ2FuaXphdGlvbl9tZW1iZXJzIHJlYWQ6b3JnYW5pemF0aW9uX21lbWJlcnMgZGVsZXRlOm9yZ2FuaXphdGlvbl9tZW1iZXJzIGNyZWF0ZTpvcmdhbml6YXRpb25fY29ubmVjdGlvbnMgcmVhZDpvcmdhbml6YXRpb25fY29ubmVjdGlvbnMgdXBkYXRlOm9yZ2FuaXphdGlvbl9jb25uZWN0aW9ucyBkZWxldGU6b3JnYW5pemF0aW9uX2Nvbm5lY3Rpb25zIGNyZWF0ZTpvcmdhbml6YXRpb25fbWVtYmVyX3JvbGVzIHJlYWQ6b3JnYW5pemF0aW9uX21lbWJlcl9yb2xlcyBkZWxldGU6b3JnYW5pemF0aW9uX21lbWJlcl9yb2xlcyBjcmVhdGU6b3JnYW5pemF0aW9uX2ludml0YXRpb25zIHJlYWQ6b3JnYW5pemF0aW9uX2ludml0YXRpb25zIGRlbGV0ZTpvcmdhbml6YXRpb25faW52aXRhdGlvbnMiLCJndHkiOiJjbGllbnQtY3JlZGVudGlhbHMifQ.N8r7HV5Yce3fxggY9W7DS4skrHdQoUUt3pI-aOrIUnBmglzk-01B_Lgv5hoBEIoIjXO048Qif-Ot3o5YyLR9JlrUwblsV6lJAoatDnN1gUHqUM6lPvSVyME9_DF4DZ8gpaQjPMuPYyHT_brPhk4lxx07FbnVZ9LF6UrOLMQTywoO5g3QM0-X2YgPXV1V-JVEaI7sUbhwSl3dlhyKbjCuTKdNbe8QsEk4CPWo5dXK2W6eeuc7Z7t7gAE6fwnFpmEE5ogsQVapVoisAMQ_4pTkTWs5cHigYwzXYmFG1lbLXEYpehaxoxnK0cHq9fky0ejtEhBu6E546iAaM6OuD5FreA");
                var response = JsonConvert.DeserializeObject<List<Dictionary<dynamic, dynamic>>>(client.Execute(request).Content);
                return response[0]["user_id"];
            }
            catch { }
            return null;
            
        }
        [HttpGet("GetRooms/")]
        [Authorize]
        public ActionResult GetRooms()
        {
            string? uuid = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var listOfRooms = getUserListOfRooms(uuid);

            /*var listOfRooms = db.Rooms
                .Include(room => room.devices)
                .Include(room => room.usersRooms)
                .Where(room => room.us)
                .Select(x => DataModelsConversionUtils.RoomFromTRoom(x))
                .ToList();*/
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
            string? email = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            db.Rooms.Add(new TRoom()
            { 
                roomName = room.roomName,
                roomDescription = room.roomDescription,
                roomImage = room.roomImage,
            });
            
            if (db.SaveChanges() > 0)
            {
                int roomId = db.Rooms.OrderBy(room=>room.roomId).Last().roomId;
                ShareRoom(new UserRoomRequest { email = email, roomId=roomId });
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
        [HttpPost("AddDevicesToConfigure/")]
        [Authorize]
        public async Task<ActionResult> AddDevicesToConfigure([FromBody] NewDeviceToConfigureRequest deviceToConfigure)
        {
            string? ip= HttpContext.Connection.RemoteIpAddress?.ToString();
            if(ip == "127.0.0.1" || ip== "::1")
            {
                var httpClient = new HttpClient();
                ip = await httpClient.GetStringAsync("https://api.ipify.org");
            }
            
            if (deviceToConfigure != null)
            {
                db.DevicesToConfigure.Add(new TDeviceToConfigure
                {
                    deviceId = deviceToConfigure.deviceId,
                    deviceType = deviceToConfigure.deviceType,
                    ipAddress = ip,
                });
                db.SaveChanges();
                return Ok(new HTTPResponse { status = 200 });
            }
            return NotFound(new HTTPResponse { status = 404 });

        }
        [HttpGet("GetDevicesToConfigure/")]
        [Authorize]
        public async Task<ActionResult> GetDevicesToConfigureAsync()
        {

            var httpClient = new HttpClient();
            var ip = await httpClient.GetStringAsync("https://api.ipify.org");

            var devicesToConfigure = db.DevicesToConfigure.Where(device => device.ipAddress == ip).ToList();
            if (devicesToConfigure != null)
            {
                return Ok(devicesToConfigure);
            }
            return NotFound(new HTTPResponse { status = 404 });
        }
        [HttpPost("AddDevice/{id}")]
        [Authorize]
        public ActionResult AddDevice(int id, [FromBody] Device device)
        {
            if (device != null)
            {
                db.Add(new TDevice
                {
                    deviceId = device.deviceId,
                    deviceName = device.deviceName,
                    deviceType = device.deviceType,
                    deviceData = device.deviceData,
                    roomId = device.roomId
                });
                var deviceConfigurationToRemove = db.DevicesToConfigure.Where(device => device.id==id).FirstOrDefault();
                if (deviceConfigurationToRemove != null)
                {
                    db.DevicesToConfigure.Remove(deviceConfigurationToRemove);
                }
                db.SaveChanges();
                return Ok(new HTTPResponse { status = 200 });
            }
            return NotFound(new HTTPResponse { status = 404 });
        }
        [HttpGet("GetRoomsCount")]
        [Authorize]
        public ActionResult GetRoomsCount()
        {
            string? uuid = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            int roomsCount = getUserListOfRooms(uuid).Count();
            return Ok(new { roomsCount});
        }
        [HttpGet("GetDevicesCount")]
        [Authorize]
        public ActionResult GetDevicesCount()
        {
            string? uuid = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            int devicesCount = 0;
            getUserListOfRooms(uuid).ForEach(room =>
            {
                devicesCount += room.devices.Count;
            });
            return Ok(new { devicesCount });
        }
        [HttpPost("ShareRoom")]
        [Authorize]
        public ActionResult ShareRoom([FromBody] UserRoomRequest userRoom)
        {
            string? uuid = GetUUIDFromEmail(userRoom.email);
            if (uuid == null)
            {
                return NotFound(new HTTPResponse { status = 404 });
            }
            db.UsersRooms.Add(new (){ 
                uuid = uuid,
                roomId = userRoom.roomId,
            });
            if (db.SaveChanges() >= 1)
            {
                return Ok(new HTTPResponse { status = 200 });
            }
            return NotFound(new HTTPResponse { status = 404 });
        }
        
    }
}
