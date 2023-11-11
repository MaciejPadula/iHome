import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { GenericResponse } from '../../shared/models/generic-response';
import { Schedule } from '../../shared/models/schedule';
import { ScheduleDevice } from '../../shared/models/schedule-device';
import { AddOrUpdateScheduleDeviceResponse } from 'src/app/shared/models/add-or-update-schedule-device-response';

@Injectable({
  providedIn: 'root'
})
export class SchedulesService {
  private readonly _baseApiUrl = `${environment.authAudience}/Schedule/`;

  constructor(
    private _api: HttpClient
  ) { }

  public addSchedule(scheduleName: string, scheduleTime: string): Observable<GenericResponse> {
    return this._api.post<GenericResponse>(`${this._baseApiUrl}AddSchedule`, {
      scheduleName,
      day: 0,
      scheduleTime
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

  public addOrUpdateScheduleDevice(scheduleId: string, deviceId: string, deviceData: string): Observable<AddOrUpdateScheduleDeviceResponse>{
    return this._api.post<AddOrUpdateScheduleDeviceResponse>(`${this._baseApiUrl}AddOrUpdateScheduleDevice`, {
      scheduleId,
      deviceId,
      deviceData
    });
  }

  public updateSchedule(scheduleId: string, scheduleTime: string): Observable<GenericResponse>{
    return this._api.post<GenericResponse>(`${this._baseApiUrl}UpdateSchedule`, {
      scheduleId,
      day: 0,
      scheduleTime
    });
  }

  public removeSchedule(scheduleId: string): Observable<GenericResponse> {
    return this._api.delete<GenericResponse>(`${this._baseApiUrl}RemoveSchedule/${scheduleId}`);
  }
}
