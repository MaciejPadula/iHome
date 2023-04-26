import { Component, Input } from '@angular/core';
import { DeviceType } from 'src/app/models/device-type';

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
}
