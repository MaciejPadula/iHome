import { Component, Input, OnInit } from '@angular/core';
import { Device } from 'src/app/models/device';

@Component({
  selector: 'app-termometer',
  templateUrl: './termometer.component.html',
  styleUrls: ['./termometer.component.scss']
})
export class TermometerComponent implements OnInit {
  @Input() device: Device = {
    deviceId: '',
    deviceName: '',
    deviceData: '',
    deviceType: 2
  };
  constructor() { }

  ngOnInit(): void {
  }

}
