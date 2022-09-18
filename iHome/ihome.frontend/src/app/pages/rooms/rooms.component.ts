import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { Device } from 'src/app/models/device';
import { Room } from 'src/app/models/room';
import { RoomsApiService } from 'src/app/services/rooms-api.service';
import { SignalRService } from 'src/app/services/signal-r.service';

interface RoomDeviceData{
  devices: Array<Device>;
  roomId: string;
}

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrls: ['./rooms.component.scss']
})
export class RoomsComponent implements OnInit {
  public rooms: Array<Room> = [];
  uuid: string = "";
  spinnerVisible = false;

  constructor(private _api: RoomsApiService, private _auth: AuthService, private _signalR: SignalRService) {
    
  }
  
  public ngOnInit() {
    this._signalR.addQuery("updateView", async () => {
      this.spinnerVisible = true;
      this.rooms = await this._api.getRooms();
      this.spinnerVisible = false;
    });
    this._signalR.runConnection();
    this._auth.user$.subscribe(u => {
      if(u?.sub != undefined){
        this.uuid = u.sub;
      }
    });
    this._signalR.callToResetView();
  }

  public drop(event: CdkDragDrop<RoomDeviceData>){
    if(event.previousContainer != event.container){
      const deviceId = event.item.data;
      const roomId = event.container.data.roomId;
      this._api.setDeviceRoom(roomId, deviceId);
      transferArrayItem(
        event.previousContainer.data.devices,
        event.container.data.devices,
        event.previousIndex,
        event.currentIndex
      );
    }
  }

  public sortDevices(devices: Array<Device>): Array<Device>{
    return devices.sort((a, b) => a.name.localeCompare(b.name));
  }
}
