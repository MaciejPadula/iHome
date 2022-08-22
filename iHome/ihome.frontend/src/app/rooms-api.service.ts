import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Room } from './room';
import { User } from './user';

@Injectable({
  providedIn: 'root'
})
export class RoomsApiService {
  private _apiUrl = 'https://localhost:7223/api/Rooms/';
  constructor(public http: HttpClient) { }
  public getRooms() {
    // const token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IkxKdHR5NlBhNnNHZ3lMb0dzX2pYcCJ9.eyJpc3MiOiJodHRwczovL2Rldi1lN2V5ajR4Zy5ldS5hdXRoMC5jb20vIiwic3ViIjoiYXV0aDB8NjJlZDMyZDQwMzE5MTE4ZTJiMDk1MDIyIiwiYXVkIjpbImh0dHBzOi8vbG9jYWxob3N0OjcyMjMvYXBpL1Jvb21zIiwiaHR0cHM6Ly9kZXYtZTdleWo0eGcuZXUuYXV0aDAuY29tL3VzZXJpbmZvIl0sImlhdCI6MTY2MTA3MzQyMywiZXhwIjoxNjYxMTU5ODIzLCJhenAiOiJlRkhwb01GRmRDN0dYSWZpOXhlNlZyWjVaMDd4S2wxMSIsInNjb3BlIjoib3BlbmlkIHByb2ZpbGUgZW1haWwifQ.Jj8rR-LnXCZSZMPA4KkCRjE_6uwePS0ES60v8DTy6YSWGV1-PSXawkG_dIyMm0whOh_x-URSuCUJBKwLMkWilv9ros1n8gxS1je5na7VjmcpZmKXO0zyeIShrDFpWZAoIMgb2uQ2fNQQX587ARiyHGSNg9MaMMrj4QxDeT4WEV9lqhEZQ4-3SE4oBzfysQjW8I78ZZoySEvDxYSKScQnwMPYAixaxIuGJLi1UckblNSQkZ6UoHt-hudBghEgfqvsve3AQcTPxYqByRYww518_u_yWQxBhrGGMCGhnVpPOqNJ2jpICyB88mv8fwaWc7yf_WAYPoby-Px5Q-VT-ixwVg";
    // axios.get(this._apiUrl+'GetRooms', { headers: {"Authorization" : `Bearer ${token}`} }).then(res => console.log(res));
    return this.http.get<Array<Room>>(this._apiUrl+'GetRooms');
  }
  public removeRoom(roomId: number){
    return this.http.post(this._apiUrl+'RemoveRoom/'+roomId, {});
  }
  public addRoom(roomName: string, roomDescription: string){
    return this.http.post(this._apiUrl+'AddRoom', {
      roomName: roomName,
      roomDescription: roomDescription,
      roomImage: ""
    });
  }
  public getRoomShares(roomId: number){
    return this.http.get<Array<User>>(this._apiUrl+'GetRoomUsers/'+roomId);
  }
  public getEmailsTest(emailTests: string){
    return this.http.get<Array<string>>(this._apiUrl + 'GetEmails/' + emailTests);
  }
  public removeRoomShare(roomId: number, uuid: string){
    return this.http.post<Array<string>>(this._apiUrl + 'RemoveRoomShare', {
      roomId: roomId,
      uuid: uuid
    });
  }
}
