import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Schedule } from '../models/schedule';
import { ScheduleDevice } from '../models/schedule-device';

@Injectable({
  providedIn: 'root'
})
export class SchedulesService {
  private readonly _baseApiUrl = `${environment.authAudience}/Schedule/`;

  constructor(
    private _api: HttpClient
  ) { }

  public getSchedules(): Observable<Schedule[]> {
    return this._api.get<Schedule[]>(`${this._baseApiUrl}GetSchedules`);
  }

  public getScheduleDevices(scheduleId: string): Observable<ScheduleDevice[]> {
    return this._api.get<ScheduleDevice[]>(`${this._baseApiUrl}GetScheduleDevices/${scheduleId}`);
  }

  public addOrUpdateScheduleDevice(scheduleId: string, deviceId: string, deviceData: string): Observable<any>{
    return this._api.post(`${this._baseApiUrl}AddOrUpdateScheduleDevice`, {
      scheduleId,
      deviceId,
      deviceData
    });
  }
}
