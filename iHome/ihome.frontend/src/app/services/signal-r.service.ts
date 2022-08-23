import { Injectable } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import * as signalR from "@microsoft/signalr";
import { AppSettingsService } from './app-settings.service';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: signalR.HubConnection;
  private _queries: { [key: string]: Function } = {};

  constructor(private _auth: AuthService, private _appSettings: AppSettingsService) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(_appSettings.BackendUrl + 'roomsHub')
      .build();
  }

  public runConnection(){
    if(this.hubConnection.state != signalR.HubConnectionState.Connected){
      this.hubConnection
      .start()
      .then(() => {
        this._auth.user$.subscribe(u => this.hubConnection.invoke("LoginToSignalR", u?.sub));
        this.addTransferChartDataListener();
        this.callToResetView();
      })
      .catch(err => console.log('Error while starting connection: ' + err));
    }
  }

  public callToResetView(){
    if(this.hubConnection.state == signalR.HubConnectionState.Connected){
      this.hubConnection.invoke("SendMessage", "");
    }
  }

  private addTransferChartDataListener = () => {
    this.hubConnection.on('ReceiveMessage', (query) => {
      this._queries[query]();
    });
  }

  public addQuery(key: string, callback: Function){
    this._queries[key] = callback;
  }
}
