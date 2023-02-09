import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Device } from '../models/device';

@Injectable({
  providedIn: 'root'
})
export class DevicesService {

  private readonly _baseApiUrl = 'https://localhost:32678/api/Device/';

  constructor(private _api: HttpClient) { }

  public getRoomDevices(roomId: string): Observable<Device[]> {
    return this._api.get<Device[]>(this._baseApiUrl + 'GetDevices/' + roomId);
  }
}
