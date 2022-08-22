import { Component, OnInit } from '@angular/core';
import { RoomsApiService } from '../rooms-api.service';
import { SignalRService } from '../signal-r.service';
import { Room } from '../room';

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrls: ['./rooms.component.scss']
})
export class RoomsComponent implements OnInit {
  
  public rooms: Array<Room> = [];
  constructor(public api: RoomsApiService, public signalR: SignalRService) {
    this.signalR.addQuery("updateView", () => {
      this.api.getRooms().subscribe(res => this.rooms = res);
    });
    this.signalR.runConnection();
  }
  ngOnInit(): void {
    this.signalR.callToResetView();
  }
}
