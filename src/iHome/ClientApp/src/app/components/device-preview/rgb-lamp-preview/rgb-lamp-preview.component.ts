import { Component } from '@angular/core';
import { RgbLampData } from '../../../models/rgb-lamp-data';
import { DeviceDataHelper } from 'src/app/helpers/device-data.helper';
import { StaticDevicePreviewComponent } from '../static-device-preview.component';

@Component({
  selector: 'app-rgb-lamp-preview',
  templateUrl: './rgb-lamp-preview.component.html',
  styleUrls: ['./rgb-lamp-preview.component.scss']
})
export class RgbLampPreviewComponent extends StaticDevicePreviewComponent<RgbLampData> {
  constructor(
    private _deviceDataHelper: DeviceDataHelper
  ) { 
    super();
  }

  public colorString(data: RgbLampData): string {
    console.log(data);
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
