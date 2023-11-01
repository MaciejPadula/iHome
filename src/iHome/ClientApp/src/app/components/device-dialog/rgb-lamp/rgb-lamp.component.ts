import { ChangeDetectionStrategy, Component, OnInit, forwardRef } from '@angular/core';
import { FormControl, NG_VALUE_ACCESSOR } from '@angular/forms';
import { DeviceDataHelper } from 'src/app/helpers/device-data.helper';
import { RgbLampData } from '../../../models/rgb-lamp-data';
import { ColorPickerControl } from '@iplab/ngx-color-picker';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { DeviceBaseControlComponent } from '../device-base-control.component';

@UntilDestroy()
@Component({
  selector: 'app-rgb-lamp',
  templateUrl: './rgb-lamp.component.html',
  styleUrls: ['./rgb-lamp.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => RgbLampComponent),
      multi: true
    }
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RgbLampComponent extends DeviceBaseControlComponent<RgbLampData> implements OnInit  {
  public colorPickerControl = new ColorPickerControl()
    .hidePresets()
    .hideAlphaChannel();

  public stateControl = new FormControl(false);

  constructor(
    private _deviceDataHelper: DeviceDataHelper
  ) {
    super();
  }

  public ngOnInit(): void {
    this.stateControl.setValue(this.data.state);
    this.colorPickerControl.setValueFrom(this.color);

    this.stateControl.valueChanges
      .pipe(
        untilDestroyed(this)
      )
      .subscribe(state => {
        this.data = {
          ...this.data,
          state: state ?? false
        };
      });

    this.colorPickerControl.valueChanges
      .pipe(
        untilDestroyed(this)
      )
      .subscribe(color => {
        const rgb = color.getRgba();
        this.data = {
          ...this.data,
          red: Math.round(rgb.red),
          green: Math.round(rgb.green),
          blue: Math.round(rgb.blue)
        };
      });
  }

  private get color() {
    return this._deviceDataHelper.getColorFromRgbLampData(this.data);
  }

  public get colorString() {
    return this._deviceDataHelper.getColorHexWithState(this.data);
  }

  protected get defaultData() {
    return {
      red: 0,
      green: 0,
      blue: 0,
      state: false,
      mode: 0
    };
  }
}
