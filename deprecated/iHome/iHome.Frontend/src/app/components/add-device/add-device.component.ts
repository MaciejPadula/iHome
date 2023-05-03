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
  @Input() roomId: string = ""; 

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
  roomId: string;
  spinnerVisible: boolean = false;
  private _ipAddress: string = "0.0.0.0";

  constructor(public dialogRef: MatDialogRef<AddDeviceDialogComponent>,
    private _api: RoomsApiService,
    private _http: HttpClient,
    @Inject(MAT_DIALOG_DATA) public data: string
  ) { 
    this.roomId = data;
  }

  public async ngOnInit(): Promise<void> {
    this._ipAddress = (await this.getIPAddress());
  }

  public async getDevicesToConfigure(){
    this.spinnerVisible = true;
    this.devicesToConfigure = await this._api.getDevicesToConfigure(this._ipAddress);
    this.spinnerVisible = false;
  }

  public onNoClick(): void {
    this.dialogRef.close();
  }

  private async getIPAddress(): Promise<any> {
    return (await this._http.get<any>("https://api.ipify.org/?format=json").toPromise()).ip;
  }
  
}