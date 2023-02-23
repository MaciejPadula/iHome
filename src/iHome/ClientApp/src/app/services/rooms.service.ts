import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Room } from '../models/room';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class RoomsService {

  private readonly _baseApiUrl = `${environment.authAudience}/Room/`;

  constructor(private _api: HttpClient) { }

  public getRooms(): Observable<Room[]> {
    return this._api.get<Room[]>(this._baseApiUrl + 'GetRooms');
  }

  public addRoom(roomName: string): Observable<object>{
    return this._api.post(this._baseApiUrl + 'AddRoom', {
      roomName
    });
  }

  public getRoomUsers(roomId: string): Observable<User[]>{
    return this._api.get<User[]>(this._baseApiUrl + `GetRoomUsers/${roomId}`);
  }

  public shareRoom(roomId: string, userId: string): Observable<object>{
    return this._api.post<string[]>(this._baseApiUrl + 'ShareRoom', {
      roomId,
      userId
    });
  }

  public unshareRoom(roomId: string, userId: string): Observable<object>{
    return this._api.post<string[]>(this._baseApiUrl + 'UnshareRoom', {
      roomId,
      userId
    });
  }

  public removeRoom(roomId: string): Observable<object> {
    return this._api.delete(this._baseApiUrl + 'RemoveRoom/' + roomId);
  }
}
