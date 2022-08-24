import { InputOutputPropertySet } from "@angular/compiler";
import { Device } from "./device";

export interface Room {
    roomId: number,
    roomName: string,
    roomDescription: string,
    masterUuid: string,
    devices: Array<Device>

}
