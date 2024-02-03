import { Component, OnInit, Inject, ChangeDetectionStrategy } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { DeviceDialogData } from './device-dialog-data';
import { DeviceType } from 'src/app/shared/models/device-type';
import { Device } from 'src/app/shared/models/device';

@UntilDestroy()
@Component({
  selector: 'app-device-dialog',
  templateUrl: './device-dialog.component.html',
  styleUrls: ['./device-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DeviceDialogComponent implements OnInit {
  public deviceDataControl: FormControl<string | null | undefined>;

  type = DeviceType;

  constructor(
    public dialogRef: MatDialogRef<DeviceDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public deviceDialogData: DeviceDialogData
  ) { }

  ngOnInit(): void {
    this.deviceDataControl = new FormControl(this.device.data);

    this.dialogRef
      .backdropClick()
      .pipe(untilDestroyed(this))
      .subscribe(() => { this.closeWithoutSaving(); });
  }

  public saveChanges() {
    this.dialogRef.close(this.deviceDataControl.value);
  }

  public closeWithoutSaving() {
    this.dialogRef.close(null);
  }

  public get device() : Device {
    return this.deviceDialogData.device;
  }
  
  public get showSchedules() : boolean {
    return this.deviceDialogData.showSchedules;
  }
  
}
