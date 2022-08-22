import { Injectable } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import * as signalR from "@microsoft/signalr";

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: signalR.HubConnection;
  private _queries: { [key: string]: Function } = {};
  constructor(public auth: AuthService) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7223/roomsHub')
      .build();
  }
  public runConnection(){
    if(this.hubConnection.state != signalR.HubConnectionState.Connected){
      this.hubConnection
      .start()
      .then(() => {
        this.auth.user$.subscribe(u => this.hubConnection.invoke("LoginToSignalR", u?.sub));
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
      console.log(query);
      this._queries[query]();
    });
  }
  public addQuery(key: string, callback: Function){
    this._queries[key] = callback;
  }
}
