import { Component } from '@angular/core';
import { faThermometerFull } from '@fortawesome/free-solid-svg-icons';
import { DeviceDataHelper } from 'src/app/helpers/device-data.helper';
import { ThermometerData } from 'src/app/models/thermometer-data';
import { ListeningDevicePreviewComponent } from '../listening-device-preview.component';
import { DevicesService } from 'src/app/services/devices.service';

@Component({
  selector: 'app-thermometer-preview',
  templateUrl: './thermometer-preview.component.html',
  styleUrls: ['./thermometer-preview.component.scss']
})
export class ThermometerPreviewComponent extends ListeningDevicePreviewComponent<ThermometerData> {
  faThermometer = faThermometerFull;

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
