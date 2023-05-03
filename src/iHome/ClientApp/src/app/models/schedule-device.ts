import { Device } from "./device";

export interface ScheduleDevice {
    id: string;
    name: string;
    deviceId: string;
    deviceData: string;

    device: Device;
}
