using iHome.Core.Logic.UserInfo;
using iHome.Core.Models.ApiRooms;
using iHome.Core.Models.Requests;
using iHome.Core.Services.DatabaseService;
using iHome.Logic.Notificator;
using iHome.Models.Account.Rooms.Requests;
using iHome.Models.Requests;
using iHome.Models.RoomsApi.Requests;
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
        public async Task<ActionResult> GetRooms()
        {
            string? uuid = _userInfo.GetUserUuid(User);
            if (uuid == null) return NotFound();

            var listOfRooms = await _databaseService.GetListOfRooms(uuid);

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

            _databaseService.AddRoom(room.RoomName, room.RoomDescription, uuid).GetAwaiter().GetResult();
            _notificator.NotifyUser(uuid);
            return Ok(new { status = 200 });
        }

        [HttpPost("RemoveRoom/{id}")]
        [Authorize]
        public async Task<ActionResult> RemoveRoom(int id)
        {
            string? uuid = _userInfo.GetUserUuid(User);
            if (uuid == null) return NotFound();
            var uuids = await _databaseService.GetRoomUserIds(id);

            await _databaseService.RemoveRoom(id);
            _notificator.NotifyUsers(uuids);
            return Ok(new { status = 200 });
        }

        [HttpPost("ShareRoom")]
        [Authorize]
        public async Task<ActionResult> ShareRoom([FromBody] UserRoomRequest userRoom)
        {
            string? uuid = _userInfo.GetUserUuid(userRoom.Email);
            if (uuid == null) return NotFound(new { exception = "Uuid equals null" });

            await _databaseService.ShareRoom(userRoom.RoomId, uuid);
            _notificator.NotifyUsers(await _databaseService.GetRoomUserIds(userRoom.RoomId));
            return Ok(new { status = 200 });
        }

        [HttpGet("GetDevices/{roomId}")]
        [Authorize]
        public async Task<ActionResult> GetDevices(int roomId)
        {
            var devices = await _databaseService.GetDevices(roomId);
            return Ok(devices);
        }

        [HttpPost("AddDevice/{id}")]
        [Authorize]
        public async Task<ActionResult> AddDevice(int id, [FromBody] Device device)
        {
            string? uuid = _userInfo.GetUserUuid(User);
            if (uuid == null) return NotFound();
            if (device == null) return NotFound(new { exception = "Wrong input data" });

            await _databaseService.AddDevice(id, device.Id, device.Name, device.Type, device.Data, device.RoomId);
            _notificator.NotifyUsers(await _databaseService.GetRoomUserIds(device.RoomId));
            return Ok(new { status = 200 });
        }

        [HttpPost("RenameDevice/")]
        [Authorize]
        public async Task<ActionResult> RenameDevice([FromBody] RenameDeviceRequest renameDevice)
        {
            string? uuid = _userInfo.GetUserUuid(User);
            if (uuid == null) return NotFound();

            if (renameDevice == null) return NotFound(new { exception = "Wrong input data" });

            await _databaseService.RenameDevice(renameDevice.DeviceId, renameDevice.DeviceName, uuid);
            _notificator.NotifyUsers(await _databaseService.GetRoomUserIds(await _databaseService.GetDeviceRoomId(renameDevice.DeviceId)));
            return Ok(new { status = 200 });
        }

        [HttpGet("GetDeviceData/{deviceId}")]
        public async Task<ActionResult> GetDeviceData(string deviceId)
        {
            string? uuid = _userInfo.GetUserUuid(User);
            if (uuid == null) return NotFound();

            var deviceData = await _databaseService.GetDeviceData(deviceId, uuid);
            return Ok(deviceData);
        }
        [HttpPost("SetDeviceData/")]
        [Authorize]
        public async Task<ActionResult> SetDeviceData([FromBody] DeviceDataRequest deviceData)
        {
            string? uuid = _userInfo.GetUserUuid(User);
            if (uuid == null) return NotFound();
            var uuids = await _databaseService.GetRoomUserIds(await _databaseService.GetDeviceRoomId(deviceData.DeviceId));

            await _databaseService.SetDeviceData(deviceData.DeviceId, deviceData.DeviceData, uuid);
            _notificator.NotifyUsers(uuids, new() { uuid });
            return Ok(new { status = 200 });
        }

        [HttpPost("SetDeviceRoom/")]
        [Authorize]
        public async Task<ActionResult> SetDeviceRoom([FromBody] NewDeviceRoomRequest newDeviceRoom)
        {
            var oldRoomId = await _databaseService.GetDeviceRoomId(newDeviceRoom.DeviceId);
            string uuid = _userInfo.GetUserUuid(User);
            if (uuid == null) return NotFound();
            var uuids = await _databaseService.GetRoomUserIds(oldRoomId);
            uuids.AddRange(await _databaseService.GetRoomUserIds(newDeviceRoom.RoomId));
            
            await _databaseService.SetDeviceRoom(newDeviceRoom.DeviceId, newDeviceRoom.RoomId, _userInfo.GetUserUuid(User));
            _notificator.NotifyUsers(uuids.Distinct().ToList(), new() { uuid });
            return Ok(new { status = 200 });
        }

        [HttpGet("GetUserId/")]
        [Authorize]
        public ActionResult GetUserId()
        {
            return Ok(_userInfo.GetUserUuid(User));
        }

        [HttpPost("GetDevicesToConfigure/")]
        [Authorize]
        public async Task<ActionResult> GetDevicesToConfigure([FromBody] GetDevicesToConfigureRequest getDevicesToConfigure)
        {
            var devicesToConfigure = await _databaseService.GetDevicesToConfigure(getDevicesToConfigure.Ip);
            return Ok(devicesToConfigure);
        }

        [HttpGet("GetEmail/{uuid}")]
        [Authorize]
        public ActionResult GetEmail(string uuid)
        {
            return Ok(_userInfo.GetUserEmail(uuid));
        }

        [HttpGet("GetRoomUsers/{roomId}")]
        [Authorize]
        public async Task<ActionResult> GetRoomUsers(int roomId)
        {
            var masterUuid = _userInfo.GetUserUuid(User);
            var uuids = (await _databaseService.GetRoomUserIds(roomId)).Where(uuid => uuid != masterUuid).ToList();
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
        public async Task<ActionResult> RemoveRoomShare([FromBody] RemoveRoomShareRequest removeShare)
        {
            var uuid = _userInfo.GetUserUuid(User);
            var uuids = await _databaseService.GetRoomUserIds(removeShare.RoomId);

            await _databaseService.RemoveRoomShare(removeShare.RoomId, removeShare.Uuid, uuid);
            _notificator.NotifyUsers(uuids);
            return Ok(new { status = 200 });
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
