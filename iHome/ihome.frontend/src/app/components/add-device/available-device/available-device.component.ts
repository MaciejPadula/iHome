import { Component, Input, OnInit } from '@angular/core';
import { Device } from 'src/app/models/device';
import { DeviceToConfigure } from 'src/app/models/device-to-configure';
import { DeviceType } from 'src/app/models/enums/device-type';
import { RGBModes } from 'src/app/models/enums/rgb-lamp-modes';
import { RoomsApiService } from 'src/app/services/rooms-api.service';

@Component({
  selector: 'app-available-device',
  templateUrl: './available-device.component.html',
  styleUrls: ['./available-device.component.scss']
})
export class AvailableDeviceComponent implements OnInit {
  @Input() roomId: number = 0;
  @Input() deviceToConfigure: DeviceToConfigure = {
    id: 0,
    deviceId: '',
    deviceType: DeviceType.RGBLamp
  };
  newDevice: Device = {
    id: '',
    type: DeviceType.RGBLamp,
    name: '',
    data: '',
    roomId: 0
  };

  constructor(private _api: RoomsApiService) { }

  ngOnInit(): void {
    this.newDevice.id = this.deviceToConfigure.deviceId;
    this.newDevice.type = this.deviceToConfigure.deviceType
    this.newDevice.roomId = this.roomId;
    switch(this.deviceToConfigure.deviceType){
      case DeviceType.RGBLamp:
        this.newDevice.data = JSON.stringify({
          Red: 255,
          Green: 255,
          Blue: 255,
          State: 1,
          Mode: RGBModes.Rainbow
        });
        break;
      case DeviceType.Termometer:
        this.newDevice.data = JSON.stringify({
          temp: 0,
          pressure: 0
        });
        break;
    }
  }

  addDevice(){
    if(this.newDevice.name.length >=3 ){
      this._api.addDevice(this.deviceToConfigure.id, this.newDevice).subscribe();
    }
  }
}
