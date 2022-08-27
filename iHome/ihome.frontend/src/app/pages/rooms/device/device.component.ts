import { Component, Input, OnInit } from '@angular/core';
import { Device } from 'src/app/models/device';

@Component({
  selector: 'app-device',
  templateUrl: './device.component.html',
  styleUrls: ['./device.component.scss']
})
export class DeviceComponent implements OnInit {
  @Input() device: Device = {
    deviceId: '',
    deviceName: '',
    deviceData: '',
    deviceType: 1
  };
  @Input() owner: boolean = false;
  constructor() { }

  ngOnInit(): void {
  }

}
