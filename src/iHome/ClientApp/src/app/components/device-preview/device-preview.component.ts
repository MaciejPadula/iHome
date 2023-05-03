import { Component, Input } from '@angular/core';
import { DeviceType } from 'src/app/models/device-type';
import { RgbLampData } from '../device/rgb-lamp/rgb-lamp-data';

@Component({
  selector: 'app-device-preview',
  templateUrl: './device-preview.component.html',
  styleUrls: ['./device-preview.component.scss']
})
export class DevicePreviewComponent {
  @Input()
  public deviceName: string;

  @Input()
  public deviceData: string;

  @Input()
  public deviceType: DeviceType;

  type = DeviceType;

  public get lampData(): RgbLampData {
    const data = JSON.parse(this.deviceData) satisfies RgbLampData;
    if(data.state) return data;

    data.red = 0;
    data.green = 0;
    data.blue = 0;
    return data;
  }
}
