import { Component, Input, OnInit } from '@angular/core';
import { Device } from 'src/app/models/device';
import { RGBColor } from 'src/app/models/rgbcolor';
import { RGBLampData } from 'src/app/models/rgblamp-data';
import { RoomsApiService } from 'src/app/services/rooms-api.service';

@Component({
  selector: 'app-rgbcontrol',
  templateUrl: './rgbcontrol.component.html',
  styleUrls: ['./rgbcontrol.component.scss']
})
export class RGBControlComponent implements OnInit {
  @Input() device: Device = {
    deviceId: '',
    deviceName: '',
    deviceData: '',
    deviceType: 1
  };
  data: RGBLampData = {
    Red: 0,
    Green: 0,
    Blue: 0,
    State: 0,
    Mode: 0
  };
  color: string = "#FFFFFF";
  state: boolean | undefined;
  mode: number = 1;

  constructor(private api: RoomsApiService) { }

  ngOnInit(): void {
    if(this.device != undefined) {
      this.data = JSON.parse(this.device.deviceData);
    }
    this.color = this.rgbToHex(this.data.Red, this.data.Green, this.data.Blue);
    this.state = Boolean(this.data.State);
  }

  updateColor(){
    const col = this.hexToRgb(this.color)
    this.data = {
      ...this.data,
      Red: col.Red,
      Green: col.Green,
      Blue: col.Blue
    }
    this.updateDevice();
  }
  
  updateState(){
    this.data = {
      ...this.data,
      State: Number(this.state)
    };
    this.updateDevice();
  }

  private updateDevice(){
    console.log(this.data);
    this.api.setDeviceData(this.device.deviceId, JSON.stringify(this.data)).subscribe();
  }

  private componentToHex(c: number): string {
    var hex = c.toString(16);
    return hex.length == 1 ? "0" + hex : hex;
  }
  
  private rgbToHex(r: number, g: number, b: number): string {
    return "#" + this.componentToHex(r) + this.componentToHex(g) + this.componentToHex(b);
  }

  private hexToRgb(hex: string): RGBColor {
    var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
    return result ? {
      Red: parseInt(result[1], 16),
      Green: parseInt(result[2], 16),
      Blue: parseInt(result[3], 16)
    } : {
      Red: 0,
      Green: 0,
      Blue: 0
    };
  }
}
