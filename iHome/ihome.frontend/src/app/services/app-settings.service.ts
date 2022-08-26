import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AppSettingsService {
  constructor() { }

  get apiUrl(){
    return environment.BackendUrl + environment.ApiSuffix
  }

  get hubUrl(){
    return environment.BackendUrl + environment.HubSuffix
  }
}
