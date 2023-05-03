using iHome.Core.Logic.UserInfo;
using iHome.Core.Models.ApiRooms;
using iHome.Core.Models.Requests;
using iHome.Core.Services.DevicesService;
using iHome.Core.Services.RoomsService;
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
        private readonly IRoomsService _roomsService;
        private readonly IDevicesService _devicesService;
        private readonly IUserInfo _userInfo;
        private readonly INotificator _notificator;

        public RoomsController(IRoomsService roomsService, IDevicesService devicesService, IUserInfo userInfo, INotificator notificator)
        {
            _roomsService = roomsService;
            _devicesService = devicesService;
            _userInfo = userInfo;
            _notificator = notificator;
        }

        [HttpGet("GetRooms/")]
        [Authorize]
        public async Task<ActionResult> GetRooms()
        {
            string uuid = _userInfo.GetUserUuid(User);

            var listOfRooms = await _roomsService.GetRooms(uuid);

            return Ok(listOfRooms);
        }

        [HttpPost("AddRoom/")]
        [Authorize]
        public async Task<ActionResult> AddRoom([FromBody()] RoomRequest room)
        {
            room.Validate();
            string uuid = _userInfo.GetUserUuid(User);

            await _roomsService.AddRoom(room.RoomName, room.RoomDescription, uuid);
            _notificator.NotifyUser(uuid);

            return Ok();
        }

        [HttpPost("RemoveRoom/{id}")]
        [Authorize]
        public async Task<ActionResult> RemoveRoom(Guid id)
        {
            var uuids = await _roomsService.GetRoomUserIds(id);

            await _roomsService.RemoveRoom(id);
            _notificator.NotifyUsers(uuids);

            return Ok();
        }

        [HttpPost("ShareRoom")]
        [Authorize]
        public async Task<ActionResult> ShareRoom([FromBody] UserRoomRequest userRoom)
        {
            string uuid = _userInfo.GetUserUuid(userRoom.Email);

            await _roomsService.AddUserRoomConstraint(userRoom.RoomId, uuid);
            _notificator.NotifyUsers(await _roomsService.GetRoomUserIds(userRoom.RoomId));

            return Ok();
        }

        [HttpPost("AddDevice/{id}")]
        [Authorize]
        public async Task<ActionResult> AddDevice(Guid id, [FromBody] Device device)
        {
            device.Validate();

            await _devicesService.AddDevice(id, device.Id, device.Name, device.Type, device.Data, device.RoomId);
            _notificator.NotifyUsers(await _roomsService.GetRoomUserIds(device.RoomId));

            return Ok();
        }

        [HttpPost("RenameDevice/")]
        [Authorize]
        public async Task<ActionResult> RenameDevice([FromBody] RenameDeviceRequest renameDevice)
        {
            renameDevice.Validate();
            string uuid = _userInfo.GetUserUuid(User);
            var roomId = await _devicesService.GetDeviceRoomId(renameDevice.DeviceId);
            var uuids = await _roomsService.GetRoomUserIds(roomId);

            await _devicesService.RenameDevice(renameDevice.DeviceId, renameDevice.DeviceName, uuid);
            _notificator.NotifyUsers(uuids);

            return Ok();
        }

        [HttpGet("GetDeviceData/{deviceId}")]
        public async Task<ActionResult> GetDeviceData(string deviceId)
        {
            string uuid = _userInfo.GetUserUuid(User);

            var deviceData = await _devicesService.GetDeviceData(deviceId, uuid);

            return Ok(deviceData);
        }
        [HttpPost("SetDeviceData/")]
        [Authorize]
        public async Task<ActionResult> SetDeviceData([FromBody] DeviceDataRequest deviceData)
        {
            var roomId = await _devicesService.GetDeviceRoomId(deviceData.DeviceId);
            string uuid = _userInfo.GetUserUuid(User);
            var uuids = await _roomsService.GetRoomUserIds(roomId);

            await _devicesService.SetDeviceData(deviceData.DeviceId, deviceData.DeviceData, uuid);
            _notificator.NotifyUsers(uuids, new List<string>() { uuid });

            return Ok();
        }

        [HttpPost("SetDeviceRoom/")]
        [Authorize]
        public async Task<ActionResult> SetDeviceRoom([FromBody] NewDeviceRoomRequest newDeviceRoom)
        {
            var oldRoomId = await _devicesService.GetDeviceRoomId(newDeviceRoom.DeviceId);
            string uuid = _userInfo.GetUserUuid(User);
            var uuids = await _roomsService.GetRoomUserIds(oldRoomId);
            uuids.AddRange(await _roomsService.GetRoomUserIds(newDeviceRoom.RoomId));

            await _devicesService.SetDeviceRoom(newDeviceRoom.DeviceId, newDeviceRoom.RoomId, _userInfo.GetUserUuid(User));
            _notificator.NotifyUsers(uuids.Distinct().ToList(), new List<string>() { uuid });

            return Ok();
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
            var devicesToConfigure = await _devicesService.GetDevicesToConfigure(getDevicesToConfigure.Ip);

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
        public async Task<ActionResult> GetRoomUsers(Guid roomId)
        {
            var userId = _userInfo.GetUserUuid(User);
            var uuids = (await _roomsService.GetRoomUserIds(roomId)).Where(uuid => uuid != userId).ToList();
            var users = new List<object>();
            uuids.ForEach(uuid => users.Add(new
            {
                email = _userInfo.GetUserEmail(uuid),
                uuid
            }));

            return Ok(users);
        }

        [HttpPost("RemoveRoomShare/")]
        [Authorize]
        public async Task<ActionResult> RemoveRoomShare([FromBody] RemoveRoomShareRequest removeShare)
        {
            var uuid = _userInfo.GetUserUuid(User);
            var uuids = await _roomsService.GetRoomUserIds(removeShare.RoomId);

            await _roomsService.RemoveUserRoomConstraint(removeShare.RoomId, removeShare.Uuid, uuid);
            _notificator.NotifyUsers(uuids);

            return Ok();
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
