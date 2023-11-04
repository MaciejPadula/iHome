import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { StaticDevicePreviewComponent } from '../static-device-preview.component';
import { RgbLampData } from 'src/app/shared/models/rgb-lamp-data';
import { DeviceDataHelper } from 'src/app/shared/helpers/device-data.helper';

@Component({
  selector: 'app-rgb-lamp-preview',
  templateUrl: './rgb-lamp-preview.component.html',
  styleUrls: ['./rgb-lamp-preview.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RgbLampPreviewComponent extends StaticDevicePreviewComponent<RgbLampData> {
  @Input() public showPointer = false;

  constructor(
    private _deviceDataHelper: DeviceDataHelper
  ) {
    super();
  }

  public colorString(data: RgbLampData): string {
    return this._deviceDataHelper.getColorHexWithState(data);
  }

  protected override get defaultData(): RgbLampData {
    return {
      red: 0,
      green: 0,
      blue: 0,
      state: false,
      mode: 0
    }
  }
}
