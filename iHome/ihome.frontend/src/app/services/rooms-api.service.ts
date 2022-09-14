import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Room } from '../models/room';
import { User } from '../models/user';
import { AppSettingsService } from './app-settings.service';
import { Bill } from '../models/bill';
import { DeviceToConfigure } from '../models/device-to-configure';
import { Device } from '../models/device';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoomsApiService {
  private _apiUrl = '';

  constructor(private _http: HttpClient, private _appSettings: AppSettingsService) {
    this._apiUrl = this._appSettings.apiUrl;
   }

  public getRooms(): Promise<Room[]> {
    return firstValueFrom(this._http.get<Room[]>(this._apiUrl+'/GetRooms'));
  }

  public removeRoom(roomId: number) {
    return firstValueFrom(this._http.post(this._apiUrl+'/RemoveRoom/'+roomId, {}));
  }

  public addRoom(roomName: string, roomDescription: string) {
    return firstValueFrom(this._http.post(this._apiUrl+'/AddRoom', {
      roomName: roomName,
      roomDescription: roomDescription,
      roomImage: ""
    }));
  }

  public getRoomShares(roomId: number): Promise<User[]>{
    return firstValueFrom(this._http.get<User[]>(this._apiUrl+'/GetRoomUsers/'+roomId));
  }

  public getEmailsByFragment(emailFragment: string): Promise<string[]>{
    return firstValueFrom(this._http.get<string[]>(this._apiUrl + '/GetEmails/' + emailFragment));
  }

  public shareRoom(roomId: number, email: string) {
    return firstValueFrom(this._http.post(this._apiUrl + '/ShareRoom', {
      roomId: roomId,
      email: email
    }));
  }

  public removeRoomShare(roomId: number, uuid: string){
    return firstValueFrom(this._http.post(this._apiUrl + '/RemoveRoomShare', {
      roomId: roomId,
      uuid: uuid
    }));
  }

  public setDeviceData(deviceId: string, deviceData: string){
    return firstValueFrom(this._http.post(this._apiUrl + '/SetDeviceData', {
      deviceId: deviceId,
      deviceData: deviceData
    }));
  }

  public async getDeviceData(deviceId: string): Promise<any> {
    return firstValueFrom(this._http.get<any>(this._apiUrl + '/GetDeviceData/' + deviceId));
  }

  public setDeviceRoom(roomId: number, deviceId: string){
    return firstValueFrom(this._http.post(this._apiUrl + '/SetDeviceRoom', {
      deviceId: deviceId,
      roomId: roomId
    }));
  }

  public getDevicesToConfigure(ipAddress: string): Promise<DeviceToConfigure[]> {
    return firstValueFrom(this._http.post<DeviceToConfigure[]>(this._apiUrl + '/GetDevicesToConfigure', {
      ip: ipAddress
    }));
  }

  public addDevice(id: number, device: Device) {
    return firstValueFrom(this._http.post(this._apiUrl + '/AddDevice/' + id, device));
  }
}
