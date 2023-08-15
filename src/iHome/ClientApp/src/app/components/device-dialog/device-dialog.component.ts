import { Component, OnInit, Inject, ChangeDetectionStrategy } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Subject } from 'rxjs';
import { TimeHelper } from 'src/app/helpers/time.helper';
import { Device } from 'src/app/models/device';
import { DeviceType } from 'src/app/models/device-type';
import { Schedule } from 'src/app/models/schedule';
import { DevicesService } from 'src/app/services/devices.service';

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

  private schedulesSubject$ = new Subject<Schedule[]>();
  public schedules$ = this.schedulesSubject$.asObservable();

  constructor(
    public dialogRef: MatDialogRef<DeviceDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public device: Device,
    private _devicesService: DevicesService,
    private _timeHelper: TimeHelper
  ) { }

  ngOnInit(): void {
    this.deviceDataControl = new FormControl(this.device.data);

    this._devicesService.getSchedules(this.device.id)
      .subscribe(x => this.schedulesSubject$.next(x));

    // this.deviceDataControl.valueChanges
    //   .subscribe(d => console.log(d));

    this.dialogRef
      .backdropClick()
      .pipe(untilDestroyed(this))
      .subscribe(() => { this.closeWithoutSaving(); });
  }

  public getScheduleTime(schedule: Schedule): string {
    return this._timeHelper.getLocalDateFromUTC(schedule.hour, schedule.minute);
  }

  public saveChanges() {
    this.dialogRef.close(this.deviceDataControl.value);
  }

  public closeWithoutSaving() {
    this.dialogRef.close(null);
  }
}
