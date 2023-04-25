import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { filter, firstValueFrom, Subject } from 'rxjs';
import { TimeHelper } from 'src/app/helpers/time.helper';
import { Device } from 'src/app/models/device';
import { Schedule } from 'src/app/models/schedule';
import { DevicesService } from 'src/app/services/devices.service';
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

  public showDevices = false;

  constructor(
    private _refreshService: RefreshService,
    private _schedulesService: SchedulesService,
    private _devicesService: DevicesService,
    private _timeHelper: TimeHelper
  ) {}

  ngOnInit(): void {
    this._refreshService.refresh$
      .pipe(untilDestroyed(this))
      .subscribe(() => this.reloadSchedule());

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
        this.scheduleHour = this._timeHelper.timeFormatPipe(schedule.hour, schedule.minute);
        this.scheduleSubject$.next(schedule);
      });
  }

  public async addDeviceSnapshot(deviceId: string) {
    const data = await firstValueFrom(this._devicesService.getDeviceData<string>(deviceId));

    this._schedulesService.addOrUpdateScheduleDevice(
      this.scheduleId,
      deviceId,
      data ?? '{}'
    ).subscribe(() => this._refreshService.refreshSchedule(this.scheduleId));
  }

  public hourChanged() {
    const time = this.scheduleHour.split(':');
    debugger;
    console.log(time);
    if(time.length < 2) return;

    this._schedulesService.updateSchedule(this.scheduleId, parseInt(time[0]), parseInt(time[1]))
      .subscribe(() => this._refreshService.refreshSchedule(this.scheduleId));
  }

  // onNotify($event: any) {
  //   console.log($event);
  // }
}
