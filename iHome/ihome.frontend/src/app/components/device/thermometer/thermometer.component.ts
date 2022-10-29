import { Component, Input, OnInit } from '@angular/core';
import { defaultDevice, Device } from 'src/app/models/device';
import { defaultTermometerData, TermometerData } from 'src/app/models/termometer-data';
import { RoomsApiService } from 'src/app/services/rooms-api.service';

@Component({
  selector: 'app-thermometer',
  templateUrl: './thermometer.component.html',
  styleUrls: ['./thermometer.component.scss']
})
export class ThermometerComponent implements OnInit {
  @Input() device: Device = defaultDevice;
  public data: TermometerData = defaultTermometerData;

  constructor(private _api: RoomsApiService) { }

  ngOnInit(): void {
    setInterval(() => this.getData(), 1000);
  }

  private async getData(){
    this.data = await this._api.getDeviceData(this.device.id);
  }

}
