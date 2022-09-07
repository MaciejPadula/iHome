import { Component, Input, OnInit } from '@angular/core';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { Device } from 'src/app/models/device';
import { RGBModes } from 'src/app/models/enums/rgb-lamp-modes';
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
    id: '',
    name: '',
    data: '',
    type: 1,
    roomId: 0
  };
  data: RGBLampData;
  color: string = "#FFFFFF";
  state: boolean;
  mode: number = 1;

  constructor(private api: RoomsApiService) { 
    this.data = {
      Red: 0,
      Green: 0,
      Blue: 0,
      State: 0,
      Mode: RGBModes.Rainbow
    }
    this.state = Boolean(this.data.State);
  }

  public ngOnInit(): void {
    if(this.device != undefined) {
      this.data = JSON.parse(this.device.data);
    }
    this.state = Boolean(this.data.State);
    this.color = this.rgbToHex(this.data.Red, this.data.Green, this.data.Blue);
  }

  public updateColor(){
    const col = this.hexToRgb(this.color)
    this.data = {
      ...this.data,
      Red: col.Red,
      Green: col.Green,
      Blue: col.Blue
    }
    this.updateDevice();
  }
  
  public updateState(){
    this.data = {
      ...this.data,
      State: Number(this.state)
    };
    this.updateDevice();
  }

  public updateMode(mode: number){
    this.data = {
      ...this.data,
      Mode: mode
    };
    this.updateDevice();
  }

  private async updateDevice(){
    await this.api.setDeviceData(this.device.id, JSON.stringify(this.data));
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
