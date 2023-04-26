import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { filter, Subject } from 'rxjs';
import { TimeHelper } from 'src/app/helpers/time.helper';
import { Device } from 'src/app/models/device';
import { Schedule } from 'src/app/models/schedule';
import { RefreshService } from 'src/app/services/refresh.service';
import { SchedulesService } from 'src/app/services/schedules.service';

@UntilDestroy()
@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ScheduleComponent implements OnInit {
  @Input() public scheduleId: string;
  @Input() public devicesForScheduling: Device[] | null;

  public scheduleHour: string;

  public scheduleSubject$ = new Subject<Schedule>();
  public schedule$ = this.scheduleSubject$.asObservable();

  constructor(
    private _refreshService: RefreshService,
    private _schedulesService: SchedulesService,
    private _timeHelper: TimeHelper
  ) {}

  ngOnInit(): void {
    this._refreshService.refreshSchedule$
      .pipe(
        untilDestroyed(this),
        filter(s => s == this.scheduleId)
      )
      .subscribe(() => this.reloadSchedule());

    this.reloadSchedule();
  }

  private reloadSchedule() {
    this._schedulesService.getSchedule(this.scheduleId)
      .subscribe(schedule => {
        this.scheduleHour = this._timeHelper.getLocalDateFromUTC(schedule.hour, schedule.minute);
        this.scheduleSubject$.next(schedule);
      });
  }

  public addDeviceSnapshot(deviceId: string, deviceData?: string) {
    this._schedulesService.addOrUpdateScheduleDevice(
      this.scheduleId,
      deviceId,
      deviceData ?? '{}'
    ).subscribe(() => this._refreshService.refreshSchedule(this.scheduleId));
  }

  public hourChanged(event: string) {
    const time = event.split(':');
    const date = this._timeHelper.getDateFromTime(parseInt(time[0]), parseInt(time[1]));

    this._schedulesService.updateSchedule(this.scheduleId, date.getUTCHours(), date.getUTCMinutes())
      .subscribe(() => this._refreshService.refresh());
  }

  // onNotify($event: any) {
  //   console.log($event);
  // }
}