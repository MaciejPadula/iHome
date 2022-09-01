import { Component, Input, OnInit } from '@angular/core';
import { Device } from 'src/app/models/device';
import { DeviceType } from 'src/app/models/enums/device-type';

@Component({
  selector: 'app-device',
  templateUrl: './device.component.html',
  styleUrls: ['./device.component.scss']
})
export class DeviceComponent implements OnInit {
  @Input() device: Device = {
    id: '',
    name: '',
    data: '',
    type: DeviceType.RGBLamp,
    roomId: 0
  };
  DeviceType = DeviceType;

  @Input() owner: boolean = false;
  constructor() {
    
  }

  ngOnInit(): void {
  }
}
