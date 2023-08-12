import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { TimeHelper } from 'src/app/helpers/time.helper';
import { Device } from 'src/app/models/device';
import { Schedule } from 'src/app/models/schedule';
import { DevicesService } from 'src/app/services/devices.service';

@Component({
  selector: 'app-device-dialog',
  templateUrl: './device-dialog.component.html',
  styleUrls: ['./device-dialog.component.scss']
})
export class DeviceDialogComponent implements OnInit {
  private schedulesSubject$ = new Subject<Schedule[]>();
  public schedules$ = this.schedulesSubject$.asObservable();

  constructor(
    public dialogRef: MatDialogRef<DeviceDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public device: Device,
    private _devicesService: DevicesService,
    private _timeHelper: TimeHelper
  ) {}

  ngOnInit(): void {
    this._devicesService.getSchedules(this.device.id)
      .subscribe(x => this.schedulesSubject$.next(x));
  }

  public getScheduleTime(schedule: Schedule): string {
    return this._timeHelper.getLocalDateFromUTC(schedule.hour, schedule.minute);
  }
}
