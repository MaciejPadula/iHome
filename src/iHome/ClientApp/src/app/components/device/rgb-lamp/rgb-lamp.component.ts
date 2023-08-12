import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { DeviceDataHelper } from 'src/app/helpers/device-data.helper';
import { DevicesService } from 'src/app/services/devices.service';
import { RgbLampData } from './rgb-lamp-data';
import { Color, ColorPickerControl } from '@iplab/ngx-color-picker';
import { DeviceBaseComponent } from '../device-base.component';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

@UntilDestroy()
@Component({
  selector: 'app-rgb-lamp',
  templateUrl: './rgb-lamp.component.html',
  styleUrls: ['./rgb-lamp.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RgbLampComponent extends DeviceBaseComponent<RgbLampData> implements OnInit  {
  public colorPickerControl = new ColorPickerControl()
    .hidePresets()
    .hideAlphaChannel();

  public stateControl = new FormControl(false);

  constructor(
    private _deviceDataHelper: DeviceDataHelper,
    deviceService: DevicesService
  ) {
    super(deviceService);
  }

  public override ngOnInit(): void {
    super.ngOnInit();

    this.data$
      .pipe(untilDestroyed(this))
      .subscribe(data => this.stateControl.setValue(data.state));
  }

  public setLampState(data: RgbLampData) {
    this.setData({
      ...data,
      state: this.stateControl.value ?? false
    });
  }

  public setColor(data: RgbLampData, color: Color) {
    const rgb = color.getRgba();
    this.setData({
      ...data,
      red: rgb.red,
      green: rgb.green,
      blue: rgb.blue
    });
  }

  public getColor(data: RgbLampData){
    return this._deviceDataHelper.getColorHexWithState(data);
  }

  public get defaultData() {
    return {
      red: 0,
      green: 0,
      blue: 0,
      state: false,
      mode: 0
    };
  }
}
