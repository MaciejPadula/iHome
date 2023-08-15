import { Component, Input } from '@angular/core';
import { ScheduleDevice } from 'src/app/models/schedule-device';
import { SchedulesService } from 'src/app/services/schedules.service';
import { RefreshService } from 'src/app/services/refresh.service';
import { MatDialog } from '@angular/material/dialog';
import { of } from 'rxjs';
import { Device } from 'src/app/models/device';

@Component({
  selector: 'app-schedule-device',
  templateUrl: './schedule-device.component.html',
  styleUrls: ['./schedule-device.component.scss']
})
export class ScheduleDeviceComponent {
  @Input() public scheduleId: string;
  @Input() public scheduleDevice: ScheduleDevice;

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

  public get device(): Device {
    return {
      id: this.scheduleDevice.id,
      name: '',
      data: this.scheduleDevice.deviceData,
      type: this.scheduleDevice.type
    }
  }

  private getDialogAfterClosed() {
    // if(this.scheduleDevice.type == DeviceType.RGBLamp){
    //   return this._dialog.open(RgbLampDialogComponent, {
    //     data: {
    //       name: this.scheduleDevice.name,
    //       data: JSON.parse(this.scheduleDevice.deviceData)
    //     }
    //   })
    //   .afterClosed();
    // }

    return of(undefined);
  }
}
