import { Component, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { Room } from 'src/app/models/room';
import { RoomsApiService } from 'src/app/services/rooms-api.service';
import { SignalRService } from 'src/app/services/signal-r.service';

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrls: ['./rooms.component.scss']
})
export class RoomsComponent implements OnInit {
  public rooms: Array<Room> = [];
  uuid: string = "";
  constructor(public api: RoomsApiService, public auth: AuthService, public signalR: SignalRService) {
    this.signalR.addQuery("updateView", () => {
      this.api.getRooms().subscribe(res => this.rooms = res);
    });
    this.signalR.runConnection();
    this.auth.user$.subscribe(u => {
      if(u?.sub != undefined){
        this.uuid = u.sub;
      }
    });
  }
  
  ngOnInit(): void {
    this.signalR.callToResetView();
  }
}
