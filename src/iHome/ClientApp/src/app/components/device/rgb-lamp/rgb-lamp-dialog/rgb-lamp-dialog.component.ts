import { ChangeDetectionStrategy, Component, Inject, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ColorPickerControl } from '@iplab/ngx-color-picker';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { debounceTime } from 'rxjs';
import { DeviceDataHelper } from 'src/app/helpers/device-data.helper';
import { DevicesService } from 'src/app/services/devices.service';
import { RefreshService } from 'src/app/services/refresh.service';
import { RgbLampDialogData } from './rgb-lamp-dialog-data';

@UntilDestroy()
@Component({
  selector: 'app-rgb-lamp-dialog',
  templateUrl: './rgb-lamp-dialog.component.html',
  styleUrls: ['./rgb-lamp-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RgbLampDialogComponent implements OnInit {
  public colorPickerControl = new ColorPickerControl()
    .hidePresets()
    .hideAlphaChannel();

  public stateControl = new FormControl(false);

  private readonly debounceTime = 300;

  constructor(
    public dialogRef: MatDialogRef<RgbLampDialogComponent>,
    private _deviceService: DevicesService,
    private _refreshService: RefreshService,
    private _deviceDataHelper: DeviceDataHelper,
    @Inject(MAT_DIALOG_DATA) public data: RgbLampDialogData
  ) {}

  ngOnInit(): void {
    this.colorPickerControl.setValueFrom(
      this._deviceDataHelper.getColorFromRgbLampData(this.data.data));
    
    this.stateControl.setValue(this.data.data.state);

    this.colorPickerControl.valueChanges
      .pipe(
        untilDestroyed(this),
        debounceTime(this.debounceTime)
      )
      .subscribe(() => this.updateDeviceData());
  }

  public updateDeviceData(){
    const data = this._deviceDataHelper
      .getRgbLampData(this.colorPickerControl.value, this.stateControl.value, null);

    this._deviceService.setDeviceData(this.data.device.id, JSON.stringify(data))
      .subscribe(() => this._refreshService.refreshDevice(this.data.device.id));
  }
}
