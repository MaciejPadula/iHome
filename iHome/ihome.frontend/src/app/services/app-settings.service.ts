import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AppSettingsService {
  public BackendUrl: string = "https://ihomewebapp.azurewebsites.net/";
  public ApiSuffix: string = "api/Rooms/";
  constructor() { }
}
