import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Room } from '../models/room';

@Injectable({
  providedIn: 'root'
})
export class RoomsService {

  private readonly _baseApiUrl = 'https://localhost:32678/api/Room/';

  constructor(private _api: HttpClient) { }

  public getRooms(): Observable<Room[]> {
    return this._api.get<Room[]>(this._baseApiUrl + 'GetRooms');
  }

  public addRoom(roomName: string): Observable<object>{
    return this._api.post(this._baseApiUrl + 'AddRoom', {
      roomName
    });
  }

  public removeRoom(roomId: string): Observable<object> {
    return this._api.delete(this._baseApiUrl + 'RemoveRoom/' + roomId);
  }
}
