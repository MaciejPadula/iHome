import { DeviceType } from "./device-type";

export interface Device {
    id: string;
    name: string;
    type: DeviceType;
    data?: string;
}
