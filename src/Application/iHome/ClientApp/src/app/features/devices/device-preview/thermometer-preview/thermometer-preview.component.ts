import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ListeningDevicePreviewComponent } from '../listening-device-preview.component';
import { DevicesService } from 'src/app/services/data/devices.service';
import { DeviceDataHelper } from 'src/app/shared/helpers/device-data.helper';
import { ThermometerData } from 'src/app/shared/models/thermometer-data';

@Component({
  selector: 'app-thermometer-preview',
  templateUrl: './thermometer-preview.component.html',
  styleUrls: ['./thermometer-preview.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ThermometerPreviewComponent extends ListeningDevicePreviewComponent<ThermometerData> {
  constructor(
    devicesService: DevicesService,
    private _deviceDataHelper: DeviceDataHelper
  ) {
    super(devicesService);
  }

  public get formattedTemperature(): string {
    return this._deviceDataHelper.formattedTemperature(this.data.temperature);
  }

  public get formattedPreassure(): string {
    return this._deviceDataHelper.formattedPreassure(this.data.pressure);
  }

  protected override get defaultData(): ThermometerData {
    return {
      temperature: 0,
      pressure: 0
    }
  }
}
