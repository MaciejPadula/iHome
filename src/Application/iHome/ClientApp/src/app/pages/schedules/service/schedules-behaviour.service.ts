import { Injectable, signal } from '@angular/core';
import { Subject, switchMap } from 'rxjs';
import { DevicesService } from 'src/app/services/data/devices.service';
import { SchedulesService } from 'src/app/services/data/schedules.service';
import { Device } from 'src/app/shared/models/device';
import { Schedule } from 'src/app/shared/models/schedule';

@Injectable({
  providedIn: 'root'
})
export class SchedulesBehaviourService {
  private schedulesSubject$ = new Subject<Schedule[]>();
  public schedules$ = this.schedulesSubject$.asObservable();

  private devicesForSchedulingSubject$ = new Subject<Device[]>();
  public devicesForScheduling$ = this.devicesForSchedulingSubject$.asObservable();

  private _isLoading = signal<boolean>(false);
  public isLoading = this._isLoading.asReadonly();

  constructor(
    private _devicesService: DevicesService,
    private _schedulesService: SchedulesService
  ) { }

  public removeSchedule(scheduleId: string) {
    this._isLoading.set(true);

    this._schedulesService.removeSchedule(scheduleId)
      .subscribe(() => this.getSchedules());
  }

  public getSchedules() {
    this._isLoading.set(true);
    this._schedulesService.getSchedules()
      .pipe(
        switchMap(s => {
          this.schedulesSubject$.next(s);
          return this._devicesService.getDevicesForScheduling();
        })
      )
      .subscribe(d => {
        this.devicesForSchedulingSubject$.next(d);
        this._isLoading.set(false);
      });
    
    
  }
}
