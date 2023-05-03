﻿using iHome.Core.Models.ApiRooms;
using iHome.Core.Models.Database;

namespace iHome.Core.Helpers
{
    public static class ListExtensions
    {
        public static List<Device> ToDevicesList(this List<TDevice>? devices)
        {
            if (devices == null)
            {
                return new List<Device>();
            }

            var devicesList = new List<Device>();
            devices.ForEach(d => devicesList.Add(d.GetDevice()));
            return devicesList;
        }

        public static bool Contains(this List<TUserRoom>? usersRooms, string userId)
        {
            if (usersRooms == null || !usersRooms.Any())
                return false;

            foreach (var userRoom in usersRooms)
            {
                if (userRoom.UserId == userId)
                    return true;
            }
            return false;
        }

        public static List<Room> ToRoomModelList(this List<TRoom> rooms, string uuid)
        {
            var roomList = new List<Room>();
            rooms.ForEach(room => roomList.Add(new Room(room.RoomId, room.Name, room.Description, room.Devices.ToDevicesList(), uuid, room.UserId)));
            return roomList;
        }
    }
}