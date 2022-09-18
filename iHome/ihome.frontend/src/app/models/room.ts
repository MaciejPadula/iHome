import { Device } from "./device";

export interface Room {
    id: string,
    name: string,
    description: string,
    ownerUuid: string,
    devices: Array<Device>

}
