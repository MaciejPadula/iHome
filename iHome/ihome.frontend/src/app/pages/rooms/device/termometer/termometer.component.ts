import { Component, Input, OnInit } from '@angular/core';
import { Device } from 'src/app/models/device';
import { Room } from 'src/app/models/room';
import { TermometerData } from 'src/app/models/termometer-data';
import { RoomsApiService } from 'src/app/services/rooms-api.service';


@Component({
  selector: 'app-termometer',
  templateUrl: './termometer.component.html',
  styleUrls: ['./termometer.component.scss']
})
export class TermometerComponent implements OnInit {
  @Input() device: Device = {
    id: '',
    name: '',
    data: '',
    type: 2,
    roomId: ''
  };

  data: TermometerData = {
    temp: 0,
    pressure: 0
  };

  constructor(private _api: RoomsApiService) { }

  public async ngOnInit(): Promise<void> {
    this.data = await this._api.getDeviceData(this.device.id);
    setInterval(async () => {
      this.data = await this._api.getDeviceData(this.device.id);
    }, 3000);  
  }
}
