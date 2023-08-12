import { ChangeDetectionStrategy, Component } from '@angular/core';
import { faThermometerFull } from '@fortawesome/free-solid-svg-icons';
import { DevicesService } from 'src/app/services/devices.service';
import { ThermometerData } from './thermometer-data';
import { DeviceDataHelper } from 'src/app/helpers/device-data.helper';
import { DeviceBaseComponent } from '../device-base.component';

@Component({
  selector: 'app-thermometer',
  templateUrl: './thermometer.component.html',
  styleUrls: ['./thermometer.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ThermometerComponent extends DeviceBaseComponent<ThermometerData> {
  public faThermometer = faThermometerFull;

  constructor(
    private _deviceDataHelper: DeviceDataHelper,
    deviceService: DevicesService
  ) {
    super(deviceService);
  }

  public formattedTemperature(data: ThermometerData): string {
    return this._deviceDataHelper.formattedTemperature(data.temperature);
  }

  public formattedPreassure(data: ThermometerData): string {
    return this._deviceDataHelper.formattedPreassure(data.pressure);
  }

  protected override get defaultData(): ThermometerData {
    return {
      temperature: 0,
      pressure: 0
    }
  }
}
