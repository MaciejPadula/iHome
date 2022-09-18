using iHome.Core.Helpers;
using iHome.Core.Logic.Database;
using iHome.Core.Middleware.Exceptions;
using iHome.Core.Models.ApiRooms;
using iHome.Core.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace iHome.Core.Services.RoomsService
{
    public class RoomsService : IRoomsService
    {
        private readonly AppDbContext _dbContext;

        public RoomsService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRoom(string roomName, string roomDescription, string uuid)
        {
            var room = await _dbContext.Rooms.AddAsync(new TRoom(Guid.NewGuid(), roomName, roomDescription, uuid, new List<TUserRoom>(), new List<TDevice>()));
            await _dbContext.SaveChangesAsync();
            await AddUserRoomConstraint(room.Entity.RoomId, uuid);
        }

        public async Task AddUserRoomConstraint(Guid roomId, string uuid)
        {
            if (await UserRoomConstraintFound(roomId, uuid))
                return;
            var room = await _dbContext.Rooms.Where(room => room.RoomId == roomId).FirstOrDefaultAsync();
            if (room == null)
                throw new RoomNotFoundException();

            await _dbContext.UsersRooms.AddAsync(new TUserRoom(Guid.NewGuid(), uuid, roomId, room));
            await _dbContext.SaveChangesAsync();
        }

        public Task<List<Room>> GetRooms(string uuid)
        {
            var rooms = _dbContext.Rooms
               .Include(db => db.Devices)
               .Include(db => db.UsersRoom)
               .ToList()
               .Where(room => room.UsersRoom.Contains(uuid))
               .OrderBy(room => room.Name)
               .ToList()
               .ToRoomModelList(uuid);
            if (rooms == null)
                return Task.FromResult(new List<Room>());

            return Task.FromResult(rooms);
        }

        public async Task<List<string>> GetRoomUserIds(Guid roomId)
        {
            var userRooms = await _dbContext.UsersRooms
                .Where(userRoom => userRoom.RoomId == roomId)
                .ToListAsync();
            if (userRooms == null)
                throw new RoomNotFoundException();

            var uuids = userRooms.Select(userRoom => userRoom.UserId).ToList();
            if (uuids == null)
                throw new RoomInternalErrorException();

            return uuids;
        }

        public async Task RemoveRoom(Guid roomId)
        {
            var roomToRemove = await _dbContext.Rooms.Where(room => room.RoomId == roomId).FirstOrDefaultAsync();
            if (roomToRemove == null)
                throw new RoomNotFoundException();
            var usersRoomsToRemove = roomToRemove.UsersRoom;
            if (usersRoomsToRemove == null)
                throw new UserRoomConstraintNotFoundException();

            _dbContext.Rooms.Remove(roomToRemove);
            _dbContext.UsersRooms.RemoveRange(usersRoomsToRemove);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveUserRoomConstraint(Guid roomId, string uuid, string masterUuid)
        {
            var room = await _dbContext.Rooms.Where(room => room.RoomId == roomId).FirstOrDefaultAsync();
            if (room == null)
                throw new RoomNotFoundException();
            if (room.UserId != masterUuid)
                throw new UnauthorizedAccessException();

            var userRoomToRemove = await _dbContext.UsersRooms
                .Where(userRoom => userRoom.RoomId == roomId && userRoom.UserId == uuid)
                .FirstOrDefaultAsync();
            if (userRoomToRemove == null)
                throw new UserRoomConstraintNotFoundException();

            _dbContext.UsersRooms.Remove(userRoomToRemove);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<bool> UserRoomConstraintFound(Guid roomId, string uuid)
        {
            var first = await _dbContext.UsersRooms
                .Where(userRoom => userRoom.RoomId == roomId && userRoom.UserId == uuid).ToListAsync();
            return first.Any();
        }
    }
}
