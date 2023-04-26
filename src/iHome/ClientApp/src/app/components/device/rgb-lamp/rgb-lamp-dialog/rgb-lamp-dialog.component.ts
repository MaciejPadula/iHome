import { ChangeDetectionStrategy, Component, Inject, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ColorPickerControl } from '@iplab/ngx-color-picker';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { DeviceDataHelper } from 'src/app/helpers/device-data.helper';
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

  private _changed = false;

  constructor(
    public dialogRef: MatDialogRef<RgbLampDialogComponent>,
    private _deviceDataHelper: DeviceDataHelper,
    @Inject(MAT_DIALOG_DATA) public data: RgbLampDialogData
  ) {}

  ngOnInit(): void {
    this.colorPickerControl.setValueFrom(
      this._deviceDataHelper.getColorFromRgbLampData(this.data.data));

    this.stateControl.setValue(this.data.data.state);

    this.colorPickerControl.valueChanges
      .pipe(untilDestroyed(this))
      .subscribe(() => this._changed = true);

    this.stateControl.valueChanges
      .pipe(untilDestroyed(this))
      .subscribe(() => this._changed = true);

    this.dialogRef
      .backdropClick()
      .pipe(untilDestroyed(this))
      .subscribe(() => { this.closeDialog(); });
  }

  public closeDialog() {
    this.dialogRef.close(null);
  }

  public saveChanges(){
    if(!this._changed) {
      this.closeDialog();
      return;
    }

    const data = this._deviceDataHelper
      .getRgbLampData(this.colorPickerControl.value, this.stateControl.value, null);

    this.dialogRef.close(data);
  }
}
