import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Device } from '../models/device';

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

  public getDeviceData<T>(deviceId: string): Observable<T> {
    return this._api.post<T>(this._baseApiUrl + 'GetDeviceData', {
      deviceId
    });
  }
}