import { ScheduleDevice } from "./schedule-device";

export interface Schedule {
    id: string;
    name: string;
    hour: number;
    minute: number;
    devices: ScheduleDevice[];
}
