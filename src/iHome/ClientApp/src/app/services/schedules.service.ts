import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Schedule } from '../models/schedule';
import { ScheduleDevice } from '../models/schedule-device';
import { ScheduleDevicesCountResponse } from '../models/schedule-devices-count-response';
import { GenericResponse } from '../models/generic-response';

@Injectable({
  providedIn: 'root'
})
export class SchedulesService {
  private readonly _baseApiUrl = `${environment.authAudience}/Schedule/`;

  constructor(
    private _api: HttpClient
  ) { }

  public addSchedule(scheduleName: string, hour: number, minute: number): Observable<GenericResponse> {
    return this._api.post<GenericResponse>(`${this._baseApiUrl}AddSchedule`, {
      scheduleName,
      day: 0,
      hour,
      minute
    });
  }

  public getSchedules(): Observable<Schedule[]> {
    return this._api.get<Schedule[]>(`${this._baseApiUrl}GetSchedules`);
  }

  public getScheduleIds(): Observable<string[]> {
    return this._api.get<string[]>(`${this._baseApiUrl}GetScheduleIds`);
  }

  public getSchedule(scheduleId: string): Observable<Schedule> {
    return this._api.get<Schedule>(`${this._baseApiUrl}GetSchedule/${scheduleId}`);
  }

  public getScheduleDevices(scheduleId: string): Observable<ScheduleDevice[]> {
    return this._api.get<ScheduleDevice[]>(`${this._baseApiUrl}GetScheduleDevices/${scheduleId}`);
  }

  public addOrUpdateScheduleDevice(scheduleId: string, deviceId: string, deviceData: string): Observable<GenericResponse>{
    return this._api.post<GenericResponse>(`${this._baseApiUrl}AddOrUpdateScheduleDevice`, {
      scheduleId,
      deviceId,
      deviceData
    });
  }

  public getScheduleDevicesCount(scheduleId: string): Observable<ScheduleDevicesCountResponse> {
    return this._api.get<ScheduleDevicesCountResponse>(`${this._baseApiUrl}GetScheduleDevicesCount/${scheduleId}`);
  }

  public updateSchedule(scheduleId: string, hour: number, minute: number): Observable<GenericResponse>{
    return this._api.post<GenericResponse>(`${this._baseApiUrl}UpdateSchedule`, {
      scheduleId,
      day: 0,
      hour,
      minute
    });
  }

  public removeSchedule(scheduleId: string): Observable<GenericResponse> {
    return this._api.delete<GenericResponse>(`${this._baseApiUrl}RemoveSchedule/${scheduleId}`);
  }
}
