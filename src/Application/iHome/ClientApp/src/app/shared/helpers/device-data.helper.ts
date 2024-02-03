import { Injectable } from '@angular/core';
import { Color } from '@iplab/ngx-color-picker';
import { RgbLampData } from '../models/rgb-lamp-data';

@Injectable({
  providedIn: 'root'
})
export class DeviceDataHelper {
  public getRgbLampData(color: Color, state?: boolean | null, mode?: number | null): RgbLampData{
    const rgb = color.getRgba();
    return {
      red: rgb.red,
      green: rgb.green,
      blue: rgb.blue,
      state: state ?? false,
      mode: mode ?? 0
    }
  }

  public getRgbLampDataWithAndOverrideState(currentData: RgbLampData, state: boolean | null): RgbLampData {
    return this.getRgbLampData(
      this.getColorFromRgbLampData(currentData),
      state ?? false,
      currentData.mode
    );
  }

  public getColorFromRgbLampData(data: RgbLampData): Color {
    return new Color()
      .setRgba(Math.round(data.red), Math.round(data.green), Math.round(data.blue))
  }

  public getColorHexWithState(currentData: RgbLampData): string{
    return currentData.state ? this.getColorFromRgbLampData(currentData)
      .toHexString() :
      "#000000";
  }

  public formattedTemperature(temp: number): string{
    ///celsius:
    return this.valueWithUnit(temp.toFixed(2), '°C');
  }

  public formattedPreassure(press: number): string{
    ///eu:
    return this.valueWithUnit(press.toFixed(2), '°hPa');
  }

  public valueWithUnit(valueFormatted: string, unit: string): string{
    return `${valueFormatted} [${unit}]`;
  }
}
