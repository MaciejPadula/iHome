import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  private readonly _baseApiUrl = `${environment.authAudience}/User/`;

  constructor(private _api: HttpClient) { }

  public getUsers(searchPhrase: string): Observable<User[]>{
    return this._api.get<User[]>(this._baseApiUrl + `GetUsers/${searchPhrase}`);
  }

  public getRoomUsers(roomId: string): Observable<User[]>{
    return this._api.get<User[]>(this._baseApiUrl + `GetRoomUsers/${roomId}`);
  }
}
