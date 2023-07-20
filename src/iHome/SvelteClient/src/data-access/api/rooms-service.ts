import type { RoomModel } from "../../models/room";
import { get } from "./call-api-service";

export function getRooms(): Promise<RoomModel[]> {
    return get('Room/GetRooms');
};