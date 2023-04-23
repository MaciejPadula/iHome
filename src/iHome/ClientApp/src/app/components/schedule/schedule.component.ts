import { animate, state, style, transition, trigger } from '@angular/animations';
import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { filter, Subject } from 'rxjs';
import { TimeHelper } from 'src/app/helpers/time.helper';
import { Device } from 'src/app/models/device';
import { Schedule } from 'src/app/models/schedule';
import { ScheduleDevice } from 'src/app/models/schedule-device';
import { DevicesService } from 'src/app/services/devices.service';
import { RefreshService } from 'src/app/services/refresh.service';
import { SchedulesService } from 'src/app/services/schedules.service';

@UntilDestroy()
@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  animations: [
    trigger('openClose', [
      state('open', style({
        height: '*',
      })),
      state('closed', style({
        height: '0px',
      })),
      transition('open <=> closed', [
        animate('0.2s')
      ]),
    ]),
  ],
})
export class ScheduleComponent implements OnInit {
  @Input() public scheduleId: string;

  public scheduleHour: string;

  public scheduleSubject$ = new Subject<Schedule>();
  public schedule$ = this.scheduleSubject$.asObservable();

  public scheduleDevicesSubject$ = new Subject<ScheduleDevice[]>();
  public scheduleDevices$ = this.scheduleDevicesSubject$.asObservable();

  public devicesSubject$ = new Subject<Device[]>();
  public devices$ = this.devicesSubject$.asObservable();

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

  private reloadSchedule(){
    this._schedulesService.getSchedule(this.scheduleId)
      .subscribe(schedule => {
        this.scheduleHour = this._timeHelper.timeFormatPipe(schedule.hour, schedule.minute);
        this.scheduleSubject$.next(schedule);
      });

    this._devicesService.getDevicesForScheduling()
      .subscribe(d => this.devicesSubject$.next(d));

    if(this.showDevices) this.loadDevices();
  }

  public toggleVisibility(){
    if(this.showDevices){
      this.showDevices = !this.showDevices;
      return;
    }

    this._devicesService.getDevicesForScheduling()
      .subscribe(d => {
        this.devicesSubject$.next(d);
        this.showDevices = !this.showDevices;
        this._refreshService.refreshSchedule(this.scheduleId);
      });

    this.loadDevices();
  }

  private loadDevices(){
    this._schedulesService.getScheduleDevices(this.scheduleId)
      .subscribe(s => this.scheduleDevicesSubject$.next(s));
  }

  public async addDeviceSnapshot(deviceId: string, data: string) {
    this._schedulesService.addOrUpdateScheduleDevice(
      this.scheduleId,
      deviceId,
      data
    ).subscribe(() => this._refreshService.refreshSchedule(this.scheduleId));
  }

  public hourChanged() {
    const time = this.scheduleHour.split(':');
    console.log(time);
    if(time.length < 2) return;

    this._schedulesService.updateSchedule(this.scheduleId, parseInt(time[0]), parseInt(time[1]))
      .subscribe(() => this._refreshService.refreshSchedule(this.scheduleId));
  }

  // onNotify($event: any) {
  //   console.log($event);
  // }
}
