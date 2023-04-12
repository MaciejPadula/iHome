import { Component, Input, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { filter, firstValueFrom, Subject } from 'rxjs';
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
  styleUrls: ['./schedule.component.scss']
})
export class ScheduleComponent implements OnInit {
  @Input() public schedule: Schedule;
  public selectedDeviceId: string;

  public scheduleDevicesSubject$ = new Subject<ScheduleDevice[]>();
  public scheduleDevices$ = this.scheduleDevicesSubject$.asObservable();

  public devicesSubject$ = new Subject<Device[]>();
  public devices$ = this.devicesSubject$.asObservable();

  public showDevices = false;

  constructor(
    private _refreshService: RefreshService,
    private _schedulesService: SchedulesService,
    private _devicesService: DevicesService
  ) { }

  ngOnInit(): void {
    this._refreshService.refresh$
      .pipe(untilDestroyed(this))
      .subscribe(() => this.getDevicesList());

    this._refreshService.refreshSchedule$
      .pipe(
        untilDestroyed(this),
        filter(s => s == this.schedule.id)
      )
      .subscribe(() => this.getDevicesForScheduling());
  }

  private getDevicesForScheduling(){
    this._schedulesService.getScheduleDevices(this.schedule.id)
      .subscribe(s => this.scheduleDevicesSubject$.next(s));
  }

  private getDevicesList(){
    this._devicesService.getDevicesForScheduling()
      .subscribe(d => this.devicesSubject$.next(d));
  }

  public toggleVisibility(){
    this.showDevices = !this.showDevices;

    if(!this.showDevices) return;

    this.getDevicesList();
    this._refreshService.refreshSchedule(this.schedule.id);
  }

  public async addDeviceSnapshot() {
    const data = await firstValueFrom(this._devicesService.getDeviceData<string>(this.selectedDeviceId));

    this._schedulesService.addOrUpdateScheduleDevice(
      this.schedule.id,
      this.selectedDeviceId,
      JSON.stringify(data)
    )
      .subscribe(() => this._refreshService.refreshSchedule(this.schedule.id));
  }

  public timeFormatPipe(hour: number, minute: number){
    return `${this.formatSegment(hour)}:${this.formatSegment(minute)}`
  }

  private formatSegment(segment: number){
    const segmentString = segment.toFixed(0);
    return segmentString.length < 2 ? `0${segmentString}` : segmentString;
  }
}
