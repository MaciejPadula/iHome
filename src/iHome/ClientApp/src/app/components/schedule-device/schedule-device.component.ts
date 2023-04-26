import { Component, Input } from '@angular/core';
import { DeviceType } from 'src/app/models/device-type';
import { ScheduleDevice } from 'src/app/models/schedule-device';
import { SchedulesService } from 'src/app/services/schedules.service';
import { RefreshService } from 'src/app/services/refresh.service';
import { MatDialog } from '@angular/material/dialog';
import { RgbLampDialogComponent } from '../device/rgb-lamp/rgb-lamp-dialog/rgb-lamp-dialog.component';
import { of } from 'rxjs';

@Component({
  selector: 'app-schedule-device',
  templateUrl: './schedule-device.component.html',
  styleUrls: ['./schedule-device.component.scss']
})
export class ScheduleDeviceComponent {
  @Input() public scheduleId: string;
  @Input() public scheduleDevice: ScheduleDevice;

  public deviceType = DeviceType;

  constructor(
    private _refreshService: RefreshService,
    private _schedulesService: SchedulesService,
    private _dialog: MatDialog
  ) {}

  public saveChanges() {
    this._schedulesService.addOrUpdateScheduleDevice(
      this.scheduleId,
      this.scheduleDevice.deviceId,
      this.scheduleDevice.deviceData
    ).subscribe(() => this._refreshService.refreshSchedule(this.scheduleId));
  }

  public composeDialog() {
    this.getDialogAfterClosed().subscribe(result => {
      if(!result) return;

      this.scheduleDevice.deviceData = JSON.stringify(result);
      this.saveChanges();
    });
  }

  private getDialogAfterClosed() {
    if(this.scheduleDevice.device.type == DeviceType.RGBLamp){
      return this._dialog.open(RgbLampDialogComponent, {
        data: {
          device: this.scheduleDevice.device,
          data: JSON.parse(this.scheduleDevice.deviceData)
        }
      })
      .afterClosed();
    }

    return of(undefined);
  }
}