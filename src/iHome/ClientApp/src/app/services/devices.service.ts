import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Device } from '../models/device';
import { Schedule } from '../models/schedule';

@Injectable({
  providedIn: 'root'
})
export class DevicesService {

  private readonly _baseApiUrl = `${environment.authAudience}/Device/`;

  constructor(private _api: HttpClient) { }

  public getRoomDevices(roomId: string): Observable<Device[]> {
    return this._api.post<Device[]>(this._baseApiUrl + 'GetDevices', {
      roomId
    });
  }

  public getDevices(): Observable<Device[]> {
    return this._api.post<Device[]>(this._baseApiUrl + 'GetDevices', {});
  }

  public getDevicesForScheduling(): Observable<Device[]> {
    return this._api.post<Device[]>(this._baseApiUrl + 'GetDevicesForScheduling', {});
  }

  public getDeviceData<T>(deviceId: string): Observable<T> {
    return this._api.post<T>(this._baseApiUrl + 'GetDeviceData', {
      deviceId
    });
  }

  public getSchedules(deviceId: string) : Observable<Schedule[]> {
    return this._api.get<Schedule[]>(`${this._baseApiUrl}GetSchedules/${deviceId}`);
  }

  public setDeviceData(deviceId: string, data: string): Observable<object> {
    return this._api.post<object>(this._baseApiUrl + 'SetDeviceData', {
      deviceId,
      data
    });
  }
}
