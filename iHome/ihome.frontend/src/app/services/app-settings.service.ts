import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AppSettingsService {
  public BackendUrl: string = "http://192.168.8.4:5000/";
  public ApiSuffix: string = "api/Rooms/";
  constructor() { }
}
