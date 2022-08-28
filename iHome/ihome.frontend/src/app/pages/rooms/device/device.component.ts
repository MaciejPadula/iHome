import { Component, Input, OnInit } from '@angular/core';
import { Device } from 'src/app/models/device';

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
    type: 1,
    roomId: 0
  };
  @Input() owner: boolean = false;
  constructor() {
    
  }

  ngOnInit(): void {
  }

}
