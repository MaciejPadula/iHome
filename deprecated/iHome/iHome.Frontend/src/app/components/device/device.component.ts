import { Component, Input, OnInit } from '@angular/core';
import { defaultDevice, Device } from 'src/app/models/device';
import { DeviceType } from 'src/app/models/enums/device-type';

@Component({
  selector: 'app-device',
  templateUrl: './device.component.html',
  styleUrls: ['./device.component.scss']
})
export class DeviceComponent implements OnInit {
  @Input() device: Device = defaultDevice;
  DeviceType = DeviceType;

  constructor() { }

  ngOnInit(): void {
  }

}
