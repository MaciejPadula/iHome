import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Device } from 'src/app/shared/models/device';
import { DeviceType } from 'src/app/shared/models/device-type';

@Component({
  selector: 'app-device-preview',
  templateUrl: './device-preview.component.html',
  styleUrls: ['./device-preview.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DevicePreviewComponent {
  @Input() public device: Device;

  type = DeviceType;

  public get startingData() {
    return this.device.data ?? '{}';
  }
}
