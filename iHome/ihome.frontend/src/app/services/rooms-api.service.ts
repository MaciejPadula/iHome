import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Room } from '../models/room';
import { User } from '../models/user';
import { AppSettingsService } from './app-settings.service';
import { Bill } from '../models/bill';
import { DeviceToConfigure } from '../models/device-to-configure';
import { Device } from '../models/device';

@Injectable({
  providedIn: 'root'
})
export class RoomsApiService {
  private _apiUrl = '';

  constructor(private _http: HttpClient, private _appSettings: AppSettingsService) {
    this._apiUrl = this._appSettings.apiUrl;
   }

  public getRooms() {
    return this._http.get<Array<Room>>(this._apiUrl+'/GetRooms');
  }

  public removeRoom(roomId: number){
    return this._http.post(this._apiUrl+'/RemoveRoom/'+roomId, {});
  }

  public addRoom(roomName: string, roomDescription: string){
    return this._http.post(this._apiUrl+'/AddRoom', {
      roomName: roomName,
      roomDescription: roomDescription,
      roomImage: ""
    });
  }

  public getRoomShares(roomId: number){
    return this._http.get<Array<User>>(this._apiUrl+'/GetRoomUsers/'+roomId);
  }

  public getEmailsTest(emailTests: string){
    return this._http.get<Array<string>>(this._apiUrl + '/GetEmails/' + emailTests);
  }

  public shareRoom(roomId: number, email: string){
    return this._http.post<Array<string>>(this._apiUrl + '/ShareRoom', {
      roomId: roomId,
      email: email
    });
  }

  public removeRoomShare(roomId: number, uuid: string){
    return this._http.post<Array<string>>(this._apiUrl + '/RemoveRoomShare', {
      roomId: roomId,
      uuid: uuid
    });
  }

  public setDeviceData(deviceId: string, deviceData: string){
    return this._http.post<Array<string>>(this._apiUrl + '/SetDeviceData', {
      deviceId: deviceId,
      deviceData: deviceData
    });
  }

  public setDeviceRoom(roomId: number, deviceId: string){
    return this._http.post(this._apiUrl + '/SetDeviceRoom', {
      deviceId: deviceId,
      roomId: roomId
    });
  }

  public getDevicesToConfigure(ipAddress: string){
    return this._http.post<Array<DeviceToConfigure>>(this._apiUrl + '/GetDevicesToConfigure', {
      ip: ipAddress
    });
  }

  public getBills(){
    return this._http.get<Array<Bill>>(this._apiUrl+'/GetBills');
  }

  public addDevice(id: number, device: Device){
    return this._http.post(this._apiUrl + '/AddDevice/' + id, device);
  }
}
