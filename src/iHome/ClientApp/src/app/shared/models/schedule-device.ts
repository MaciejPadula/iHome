import { DeviceType } from "./device-type";

export interface ScheduleDevice {
    id: string;
    name: string;
    deviceId: string;
    deviceData: string;
    scheduleId: string;
    type: DeviceType
}
