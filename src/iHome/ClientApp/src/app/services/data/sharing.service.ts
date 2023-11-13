import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class SharingService {
  private readonly _baseApiUrl = `${environment.authAudience}/RoomSharing/`;

  constructor(private _api: HttpClient) { }

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

  public getRoomUsers(roomId: string): Observable<User[]>{
    return this._api.get<User[]>(this._baseApiUrl + `GetRoomUsers/${roomId}`);
  }
}
