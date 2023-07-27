import type { RoomModel } from "../../models/room";
import { httpdelete, httpget, httppost } from "./call-api-service";

const prefix = "Room/";

export function addRoom(roomName: string): Promise<string> {
    return httppost(`${prefix}AddRoom`, {
        roomName
    });
}

export function removeRoom(roomId: string) {
    return httpdelete(`${prefix}RemoveRoom/${roomId}`);
}

export function getRooms(): Promise<RoomModel[]> {
    return httpget(`${prefix}GetRooms`);
};