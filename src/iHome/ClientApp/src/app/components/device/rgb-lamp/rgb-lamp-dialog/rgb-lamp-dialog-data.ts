import { Device } from "src/app/models/device";
import { RgbLampData } from "../rgb-lamp-data";

export interface RgbLampDialogData {
    device: Device;
    data: RgbLampData;
}