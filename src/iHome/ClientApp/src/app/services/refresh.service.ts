import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RefreshService {
  private refreshSubject$ = new Subject<void>();
  private refreshDeviceSubject$ = new Subject<string>();
  private refreshRoomUsersSubject$ = new Subject<string>();
  private refreshScheduleSubject$ = new Subject<string>();

  public refresh$ = this.refreshSubject$.asObservable();
  public refreshDevice$ = this.refreshDeviceSubject$.asObservable();
  public refreshRoomUsers$ = this.refreshRoomUsersSubject$.asObservable();
  public refreshSchedule$ = this.refreshScheduleSubject$.asObservable();

  public refresh(){
    this.refreshSubject$.next();
  }

  public refreshDevice(deviceId: string){
    this.refreshDeviceSubject$.next(deviceId);
  }

  public refreshRoomUsers(roomId: string){
    this.refreshRoomUsersSubject$.next(roomId);
  }

  public refreshSchedule(scheduleId: string){
    this.refreshScheduleSubject$.next(scheduleId);
  }
}
