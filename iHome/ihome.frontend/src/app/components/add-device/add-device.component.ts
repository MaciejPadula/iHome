import { HttpClient } from '@angular/common/http';
import { Component, Inject, Input, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DeviceToConfigure } from 'src/app/models/device-to-configure';
import { RoomsApiService } from 'src/app/services/rooms-api.service';

@Component({
  selector: 'app-add-device',
  templateUrl: './add-device.component.html',
  styleUrls: ['./add-device.component.scss']
})
export class AddDeviceComponent {
  @Input() roomId: number = 0; 

  constructor(public dialog: MatDialog) { }

  openDialog() {
    const dialogRef = this.dialog.open(AddDeviceDialogComponent, {
      width: '30rem',
      data: this.roomId
    });
    dialogRef.afterClosed().subscribe();
  }

}

@Component({
  selector: 'add-device-dialog',
  templateUrl: './add-device-dialog.component.html',
  styleUrls: ['./add-device-dialog.component.scss']
})
export class AddDeviceDialogComponent implements OnInit{
  devicesToConfigure: Array<DeviceToConfigure> = [];
  roomId: number;
  private _ipAddress: string = "0.0.0.0";

  constructor(public dialogRef: MatDialogRef<AddDeviceDialogComponent>,
    private _api: RoomsApiService,
    private _http: HttpClient,
    @Inject(MAT_DIALOG_DATA) public data: number
  ) { 
    this.roomId = data;
    
  }

  async ngOnInit(): Promise<void> {
    this._ipAddress = (await this.getIPAddress());
  }

  getDevicesToConfigure(){
    this._api.getDevicesToConfigure(this._ipAddress).subscribe(res => this.devicesToConfigure = res);
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  private async getIPAddress(): Promise<any>
  {
    return (await this._http.get<any>("http://api.ipify.org/?format=json").toPromise()).ip;
  }
  
}