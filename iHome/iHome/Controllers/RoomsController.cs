using iHome.Core.Logic.UserInfo;
using iHome.Core.Models.ApiRooms;
using iHome.Core.Services.DatabaseService;
using iHome.Logic.Notificator;
using iHome.Models.Account.Rooms.Requests;
using iHome.Models.Requests;
using iHome.Models.RoomsApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
            if (_databaseService.AddRoom(room.RoomName, room.RoomDescription, uuid))
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
            string? uuid = _userInfo.GetUserUuid(userRoom.Email);
            if (uuid == null) return NotFound(new { exception = "Uuid equals null" });
            if (_databaseService.ShareRoom(userRoom.RoomId, uuid))
            {
                _notificator.NotifyUsers(_databaseService.GetRoomUserIds(userRoom.RoomId));
                return Ok(new { status = 200 });
            }
            return Ok(new { status = 801, exception = "Can't share room" });
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

            if (_databaseService.AddDevice(id, device.Id, device.Name, device.Type, device.Data, device.RoomId))
            {
                _notificator.NotifyUsers(_databaseService.GetRoomUserIds(device.RoomId));
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
            if (_databaseService.RenameDevice(renameDevice.DeviceId, renameDevice.DeviceName, uuid))
            {
                _notificator.NotifyUsers(_databaseService.GetRoomUserIds(_databaseService.GetDeviceRoomId(renameDevice.DeviceId)));
                return Ok(new { status = 200 });
            }
            return NotFound(new { exception = "Can't rename device" });
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
            if (_databaseService.SetDeviceData(deviceData.DeviceId, deviceData.DeviceData, uuid))
            {
                var uuids = _databaseService
                    .GetRoomUserIds(_databaseService.GetDeviceRoomId(deviceData.DeviceId));
                _notificator.NotifyUsers(uuids, new() { uuid });
                return Ok(new { status = 200 });
            }
            return NotFound(new { exception = "Can't set device data" });
        }

        [HttpPost("SetDeviceRoom/")]
        [Authorize]
        public ActionResult SetDeviceRoom([FromBody] NewDeviceRoomRequest newDeviceRoom)
        {
            var oldRoomId = _databaseService.GetDeviceRoomId(newDeviceRoom.DeviceId);
            string uuid = _userInfo.GetUserUuid(User);
            if (uuid == null) return NotFound();
            

            if (_databaseService.SetDeviceRoom(newDeviceRoom.DeviceId, newDeviceRoom.RoomId, _userInfo.GetUserUuid(User)))
            {
                var uuids = _databaseService.GetRoomUserIds(oldRoomId);
                uuids.AddRange(_databaseService.GetRoomUserIds(newDeviceRoom.RoomId));

                _notificator.NotifyUsers(uuids.Distinct().ToList(), new() { uuid });
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
            var devicesToConfigure = _databaseService.GetDevicesToConfigure(getDevicesToConfigure.Ip);
            if (devicesToConfigure != null)
            {
                return Ok(devicesToConfigure);
            }
            return NotFound(new { exception = "Can't get devices to configure list" });
        }

        [HttpGet("GetEmail/{uuid}")]
        [Authorize]
        public ActionResult GetEmail(string uuid)
        {
            return Ok(_userInfo.GetUserEmail(uuid));
        }

        [HttpGet("GetRoomUsers/{roomId}")]
        [Authorize]
        public ActionResult GetRoomUsers(int roomId)
        {
            var masterUuid = _userInfo.GetUserUuid(User);
            var uuids = _databaseService.GetRoomUserIds(roomId).Where(uuid => uuid != masterUuid).ToList();
            var users = new List<object>();
            uuids.ForEach(uuid => users.Add(new
            {
                email = _userInfo.GetUserEmail(uuid),
                uuid = uuid
            }));
            return Ok(users);
        }

        [HttpPost("RemoveRoomShare/")]
        [Authorize]
        public ActionResult RemoveRoomShare([FromBody] RemoveRoomShareRequest removeShare)
        {
            var uuid = _userInfo.GetUserUuid(User);
            var uuids = _databaseService.GetRoomUserIds(removeShare.RoomId);
            if (_databaseService.RemoveRoomShare(removeShare.RoomId, removeShare.Uuid, uuid))
            {
                _notificator.NotifyUsers(uuids);
                return Ok(new { status = 200 });
            }
            return NotFound(new { exception = "Can't remove room share" });
        }

        [HttpGet("GetEmails/")]
        [Authorize]
        public ActionResult GetEmails()
        {
            return Ok(new List<string>());
        }

        [HttpGet("GetEmails/{emailTest}")]
        [Authorize]
        public ActionResult GetEmails(string emailTest)
        {
            return Ok(_userInfo.GetEmails(emailTest).OrderBy(e => e).ToList());
        }
    }
}
