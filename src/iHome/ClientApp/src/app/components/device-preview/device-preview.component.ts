import { Component, Input } from '@angular/core';
import { DeviceType } from 'src/app/models/device-type';
import { RgbLampData } from '../device/rgb-lamp/rgb-lamp-data';
import { ThermometerData } from '../device/thermometer/thermometer-data';
import { DeviceDataHelper } from 'src/app/helpers/device-data.helper';
import { faThermometerFull } from '@fortawesome/free-solid-svg-icons';

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
  faThermometer = faThermometerFull;

  constructor(
    private _deviceDataHelper: DeviceDataHelper
  ) {}

  public get lampData(): RgbLampData {
    const data = JSON.parse(this.deviceData) satisfies RgbLampData;
    if(data.state) return data;

    data.red = 0;
    data.green = 0;
    data.blue = 0;
    return data;
  }

  public get thermometerData(): ThermometerData {
    return JSON.parse(this.deviceData) satisfies ThermometerData;
  }

  public formattedTemperature(temp: number): string {
    return this._deviceDataHelper.formattedTemperature(temp ?? 0);
  }

  public formattedPreassure(press: number): string {
    return this._deviceDataHelper.formattedPreassure(press ?? 0);
  }
}
