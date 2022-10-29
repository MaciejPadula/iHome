import { Device } from "./device";

export interface Room {
    id: string,
    name: string,
    description: string,
    ownerUuid: string,
    devices: Array<Device>
}

const defaultRoom: Room = {
    id: '',
    name: '',
    description: '',
    ownerUuid: '',
    devices: []
}
export {
    defaultRoom
};