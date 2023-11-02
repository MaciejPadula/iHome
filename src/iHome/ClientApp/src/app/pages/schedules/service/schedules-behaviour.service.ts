import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { DevicesService } from 'src/app/services/devices.service';
import { SchedulesService } from 'src/app/services/schedules.service';
import { Device } from 'src/app/shared/models/device';
import { Schedule } from 'src/app/shared/models/schedule';

@Injectable({
  providedIn: 'root'
})
export class SchedulesBehaviourService {
  private schedulesSubject$ = new Subject<Schedule[] | null>();
  public schedules$ = this.schedulesSubject$.asObservable();

  private devicesForSchedulingSubject$ = new Subject<Device[]>();
  public devicesForScheduling$ = this.devicesForSchedulingSubject$.asObservable();

  private isLoadingSubject$ = new Subject<boolean>();
  public isLoading$ = this.isLoadingSubject$.asObservable();

  constructor(
    private _devicesService: DevicesService,
    private _schedulesService: SchedulesService
  ) { }

  public removeSchedule(scheduleId: string) {
    this.isLoadingSubject$.next(true);

    this._schedulesService.removeSchedule(scheduleId)
      .subscribe(() => this.getSchedules());
  }

  public getSchedules() {
    this.isLoadingSubject$.next(true);
    this._schedulesService.getSchedules()
      .subscribe(s => {
        this.schedulesSubject$.next(s);
        this._devicesService.getDevicesForScheduling()
          .subscribe(d => {
            this.devicesForSchedulingSubject$.next(d);
            this.isLoadingSubject$.next(false);
          });
      });
    
    
  }
}
