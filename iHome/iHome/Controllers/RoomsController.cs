using iHome.Logic.Notificator;
using iHome.Logic.UserInfo;
using iHome.Models.Account.Rooms.Requests;
using iHome.Models.DataModels;
using iHome.Models.Requests;
using iHome.Models.RoomsApi.Requests;
using iHome.Services.DatabaseService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Controllers
{
    /// <summary>
    /// API Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly IUserInfo _userInfo;
        private readonly INotificator _notificator;
        public RoomsController(IDatabaseService databaseApi, IUserInfo userInfo, INotificator notificator)
        {
            _userInfo = userInfo;
            _databaseService = databaseApi;
            _notificator = notificator;
        }

        [HttpGet("GetRooms/")]
        [Authorize]
        public ActionResult GetRooms()
        {
            string? uuid = _userInfo.GetUserUuid(User);
            if (uuid == null) return NotFound();

            var listOfRooms = _databaseService.GetListOfRooms(uuid);

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
            string? uuid = _userInfo.GetUserUuid(User);
            if (room == null || uuid == null) return NotFound(new { exception = "room or uuid is null" });
            if (_databaseService.AddRoom(room.roomName, room.roomDescription, uuid))
            {
                _notificator.NotifyUser(uuid);
                return Ok(new { status = 200 });
            }
            return NotFound(new { exception = "Can't add new room" });
        }

        [HttpPost("RemoveRoom/{id}")]
        [Authorize]
        public ActionResult RemoveRoom(int id)
        {
            string? uuid = _userInfo.GetUserUuid(User);
            if (uuid == null) return NotFound();
            var uuids = _databaseService.GetRoomUserIds(id);
            if (_databaseService.RemoveRoom(id))
            {
                _notificator.NotifyUsers(uuids);
                return Ok(new { status = 200 });
            }
            return NotFound(new { exception = "Can't remove room" });
        }

        [HttpPost("ShareRoom")]
        [Authorize]
        public ActionResult ShareRoom([FromBody] UserRoomRequest userRoom)
        {
            string? uuid = _userInfo.GetUserUuid(userRoom.email);
            if (uuid == null) return NotFound(new { exception = "Uuid equals null" });
            if (_databaseService.ShareRoom(userRoom.roomId, uuid))
            {
                _notificator.NotifyUsers(_databaseService.GetRoomUserIds(userRoom.roomId));
                return Ok(new { status = 200 });
            }
            return Ok(new { status = 801, exception = "Can't share room" });
        }

        [HttpGet("GetRoomsCount")]
        [Authorize]
        public ActionResult GetRoomsCount()
        {
            string? uuid = _userInfo.GetUserUuid(User);
            if (uuid == null) return NotFound();
            int roomsCount = _databaseService.GetRoomsCount(uuid);
            return Ok(new { roomsCount });
        }

        [HttpGet("GetDevices/{roomId}")]
        [Authorize]
        public ActionResult GetDevices(int roomId)
        {
            var devices = _databaseService.GetDevices(roomId);
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
            string? uuid = _userInfo.GetUserUuid(User);
            if (uuid == null) return NotFound();

            if (device == null) return NotFound(new { exception = "Wrong input data" });

            if (_databaseService.AddDevice(id, device.deviceId, device.deviceName, device.deviceType, device.deviceData, device.roomId))
            {
                _notificator.NotifyUsers(_databaseService.GetRoomUserIds(device.roomId));
                return Ok(new { status = 200 });
            }
            return NotFound(new { exception = "Can't add device" });
        }

        [HttpPost("RenameDevice/")]
        [Authorize]
        public ActionResult RenameDevice([FromBody] RenameDeviceRequest renameDevice)
        {
            string? uuid = _userInfo.GetUserUuid(User);
            if (uuid == null) return NotFound();

            if (renameDevice == null) return NotFound(new { exception = "Wrong input data" });
            if (_databaseService.RenameDevice(renameDevice.deviceId, renameDevice.deviceName, uuid))
            {
                _notificator.NotifyUsers(_databaseService.GetRoomUserIds(_databaseService.GetDeviceRoomId(renameDevice.deviceId)));
                return Ok(new { status = 200 });
            }
            return NotFound(new { exception = "Can't rename device" });
        }

        [HttpGet("GetDevicesCount")]
        [Authorize]
        public ActionResult GetDevicesCount()
        {
            string? uuid = _userInfo.GetUserUuid(User);
            if (uuid == null) return NotFound(new { exception = "Uuid equals null" });
            var devicesCount = _databaseService.GetDevicesCount(uuid);
            return Ok(new { devicesCount });
        }

        [HttpGet("GetDeviceData/{deviceId}")]
        public ActionResult GetDeviceData(string deviceId)
        {
            string? uuid = _userInfo.GetUserUuid(User);
            if (uuid == null) return NotFound();

            var deviceData = _databaseService.GetDeviceData(deviceId, uuid);
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
            string? uuid = _userInfo.GetUserUuid(User);
            if (uuid == null) return NotFound();
            if (_databaseService.SetDeviceData(deviceData.deviceId, deviceData.deviceData, uuid))
            {
                var uuids = _databaseService
                    .GetRoomUserIds(_databaseService.GetDeviceRoomId(deviceData.deviceId));
                _notificator.NotifyUsers(uuids);
                return Ok(new { status = 200 });
            }
            return NotFound(new { exception = "Can't set device data" });
        }

        [HttpPost("SetDeviceRoom/")]
        [Authorize]
        public ActionResult SetDeviceRoom([FromBody] NewDeviceRoomRequest newDeviceRoom)
        {
            string? uuid = _userInfo.GetUserUuid(User);
            if (uuid == null) return NotFound();
            if (_databaseService.SetDeviceRoom(newDeviceRoom.deviceId, newDeviceRoom.roomId, _userInfo.GetUserUuid(User)))
            {
                _notificator.NotifyUsers(_databaseService.GetRoomUserIds(newDeviceRoom.roomId));
                return Ok(new { status = 200 });
            }
            return NotFound(new { exception = "Can't change device room" });
        }

        [HttpGet("GetUserId/")]
        [Authorize]
        public ActionResult GetUserId()
        {
            return Ok(_userInfo.GetUserUuid(User));
        }

        [HttpPost("GetDevicesToConfigure/")]
        [Authorize]
        public ActionResult GetDevicesToConfigure([FromBody] GetDevicesToConfigureRequest getDevicesToConfigure)
        {
            var devicesToConfigure = _databaseService.GetDevicesToConfigure(getDevicesToConfigure.ip);
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
            var ip = await _userInfo.GetPublicIp(HttpContext);
            if (ip == null || deviceToConfigure == null) return NotFound(new { exception = "IP or input data equals null" });
            if (_databaseService.AddDevicesToConfigure(deviceToConfigure.deviceId, deviceToConfigure.deviceType, ip))
            {
                return Ok(new { status = 200 });
            }
            return NotFound(new { exception = "Can't add new device to configure" });
        }

        [HttpGet("GetIP")]
        [Authorize]
        public async Task<ActionResult> GetIP()
        {
            return Ok(await _userInfo.GetPublicIp(HttpContext));
        }

        [HttpGet("GetEmail/{uuid}")]
        [Authorize]
        public async Task<ActionResult> GetEmail(string uuid)
        {
            return Ok(_userInfo.GetUserEmail(uuid));
        }

        [HttpGet("GetRoomUsers/{roomId}")]
        [Authorize]
        public async Task<ActionResult> GetRoomUsers(int roomId)
        {
            var masterUuid = _userInfo.GetUserUuid(User);
            var uuids = _databaseService.GetRoomUserIds(roomId).Where(uuid => uuid!=masterUuid).ToList();
            var users = new List<object>();
            uuids.ForEach(uuid => users.Add(new
            {
                email= _userInfo.GetUserEmail(uuid),
                uuid = uuid
            }));
            return Ok(users);
        }

        [HttpPost("RemoveRoomShare/")]
        [Authorize]
        public async Task<ActionResult> RemoveRoomShare([FromBody] RemoveRoomShareRequest removeShare)
        {
            var uuid = _userInfo.GetUserUuid(User);
            var uuids = _databaseService.GetRoomUserIds(removeShare.roomId);
            if (_databaseService.RemoveRoomShare(removeShare.roomId, removeShare.uuid, uuid))
            {
                _notificator.NotifyUsers(uuids);
                return Ok(new { status = 200 });
            }
            return NotFound(new { exception = "Can't remove room share" });
        }

        [HttpGet("GetEmails/")]
        [Authorize]
        public async Task<ActionResult> GetEmails()
        {
            return Ok(new List<string>());
        }

        [HttpGet("GetEmails/{emailTest}")]
        [Authorize]
        public async Task<ActionResult> GetEmails(string emailTest)
        {
            return Ok(_userInfo.GetEmails(emailTest).OrderBy(e => e).ToList());
        }
    }
}
