import { Component, Inject, Input, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { defaultDevice, Device } from 'src/app/models/device';
import { RGBModes } from 'src/app/models/enums/rgb-lamp-modes';
import { RGBColor } from 'src/app/models/rgbcolor';
import { RGBLampData } from 'src/app/models/rgblamp-data';
import { RoomsApiService } from 'src/app/services/rooms-api.service';

@Component({
  selector: 'app-rgb-lamp',
  templateUrl: './rgb-lamp.component.html',
  styleUrls: ['./rgb-lamp.component.scss']
})
export class RgbLampComponent implements OnInit{
  @Input() device: Device = defaultDevice;
  public data: RGBLampData = defaultLampData;

  constructor(private _api: RoomsApiService, public dialog: MatDialog) { }

  ngOnInit(): void {
    this.data = JSON.parse(this.device.data);
  }

  openDialog() {
    const dialogRef = this.dialog.open(DeviceDialogComponent, {
      width: '20rem',
      data: {
        data: this.data,
        device: this.device
      }
    });
    dialogRef.afterClosed().subscribe();
  }

  public turnState(){
    if(this.data.State) this.data.State = 0;
    else this.data.State = 1;
    this.updateDevice();
  }

  private updateDevice(){
    this._api.setDeviceData(this.device.id, JSON.stringify(this.data));
    this.device.data = JSON.stringify(this.data);
  }
}

@Component({
  selector: 'rgb-lamp-dialog',
  templateUrl: './rgb-lamp-dialog.component.html',
  styleUrls: ['./rgb-lamp-dialog.component.scss']
})
export class DeviceDialogComponent implements OnInit{
  public color: string = '#FFFFFF';

  constructor(public dialogRef: MatDialogRef<DeviceDialogComponent>, 
    @Inject(MAT_DIALOG_DATA) public data: {data: RGBLampData, device: Device},
    private _api: RoomsApiService) {}
  ngOnInit(): void {
    this.color = this.rgbToHex(this.data.data.Red, this.data.data.Green, this.data.data.Blue);
  }

  public turnState(){
    if(this.data.data.State) this.data.data.State = 0;
    else this.data.data.State = 1;
    this.updateDevice();
  }

  public updateColor(){
    const color = this.hexToRgb(this.color);
    this.data.data.Red = color.Red;
    this.data.data.Green = color.Green;
    this.data.data.Blue = color.Blue;
    this.updateDevice();
  }

  public updateMode(ev: any){
    this.data.data.Mode = ev;
    this.updateDevice();
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

  private updateDevice(){
    this._api.setDeviceData(this.data.device.id, JSON.stringify(this.data.data));
    this.data.device.data = JSON.stringify(this.data.data);
  }
  
}

const defaultLampData: RGBLampData = {
  Red: 0,
  Green: 0,
  Blue: 0,
  Mode: RGBModes.Static,
  State: 0
}
