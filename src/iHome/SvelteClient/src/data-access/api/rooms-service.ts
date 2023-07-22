import type { RoomModel } from "../../models/room";
import { httpget } from "./call-api-service";

export function getRooms(): Promise<RoomModel[]> {
    return httpget('Room/GetRooms');
};