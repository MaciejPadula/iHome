import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Subject } from 'rxjs';
import { InputDialogData } from 'src/app/shared/components/input-dialog/input-dialog-data';
import { InputDialogComponent } from 'src/app/shared/components/input-dialog/input-dialog.component';
import { TimeHelper } from 'src/app/helpers/time.helper';
import { Device } from 'src/app/models/device';
import { Schedule } from 'src/app/models/schedule';
import { DevicesService } from 'src/app/services/devices.service';
import { RefreshService } from 'src/app/services/refresh.service';
import { SchedulesService } from 'src/app/services/schedules.service';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { ConfirmDialogData } from 'src/app/shared/components/confirm-dialog/confirm-dialog-data';

@UntilDestroy()
@Component({
  selector: 'app-schedules',
  templateUrl: './schedules.component.html',
  styleUrls: ['./schedules.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SchedulesComponent implements OnInit {
  public animState = false;
  public schedulesSubject$ = new Subject<Schedule[]>();
  public schedules$ = this.schedulesSubject$.asObservable();

  public devicesForSchedulingSubject$ = new Subject<Device[]>();
  public devicesForScheduling$ = this.devicesForSchedulingSubject$.asObservable();

  constructor(
    private _refreshService: RefreshService,
    private _schedulesService: SchedulesService,
    private _devicesService: DevicesService,
    private _timeHelper: TimeHelper,
    private _dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this._refreshService.refresh$
      .pipe(untilDestroyed(this))
      .subscribe(() => this.getSchedules());

    this._refreshService.refresh();
  }

  public composeDialogAddSchedule() {
    this._dialog.open(InputDialogComponent, {
      data: <InputDialogData> {
        title: 'Add Schedule',
        inputText: 'Schedule name'
      }
    })
    .afterClosed()
    .subscribe(result => {
      if(!result) return;

      const date = new Date();

      this._schedulesService.addSchedule(result, date.getUTCHours(), date.getUTCMinutes())
        .subscribe(() => this._refreshService.refresh());
    });
  }

  public composeDialogRemoveSchedule(scheduleId: string, scheduleName: string) {
    this._dialog.open(ConfirmDialogComponent, {
      data: <ConfirmDialogData> {
        title: 'Warning',
        additionalText: `You are going to remove schedule named: ${scheduleName}. Are you sure?`
      }
    })
    .afterClosed()
    .subscribe(result => {
      if(!result) return;

      this._schedulesService.removeSchedule(scheduleId)
        .subscribe(() => this._refreshService.refresh());
    })
  }

  private getSchedules() {
    this._schedulesService.getSchedules()
      .subscribe(s => {
        this.schedulesSubject$.next(s);
        this._devicesService.getDevicesForScheduling()
          .subscribe(d => this.devicesForSchedulingSubject$.next(d));
      });
  }

  public getTimeFormatted(hour: number, minute: number){
    return this._timeHelper.getLocalDateFromUTC(hour, minute);
  }
}