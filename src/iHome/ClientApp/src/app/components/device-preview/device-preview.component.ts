import { Component, Input } from '@angular/core';
import { DeviceType } from 'src/app/models/device-type';
import { Device } from 'src/app/models/device';

@Component({
  selector: 'app-device-preview',
  templateUrl: './device-preview.component.html',
  styleUrls: ['./device-preview.component.scss']
})
export class DevicePreviewComponent {
  @Input() public device: Device;

  type = DeviceType;

  public get startingData() {
    return this.device.data ?? '{}';
  }
}
