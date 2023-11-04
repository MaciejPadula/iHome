import { Component, Input } from "@angular/core";
import { Device } from "src/app/shared/models/device";

@Component({ template: '' })
export abstract class StaticDevicePreviewComponent<DeviceDataType> {
  @Input() public device: Device;

  protected get data(): DeviceDataType {
    if (!this.device?.data) {
      return this.defaultData;
    }

    return JSON.parse(this.device.data);
  }

  protected abstract get defaultData(): DeviceDataType;
}