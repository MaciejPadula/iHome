import { Device } from "./device";

export interface Room {
    id: number,
    name: string,
    description: string,
    ownerUuid: string,
    devices: Array<Device>

}
