import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input } from '@angular/core';
import { SchedulesService } from 'src/app/services/data/schedules.service';
import { MatDialog } from '@angular/material/dialog';
import { DeviceDialogComponent } from '../../devices/device-dialog/device-dialog.component';
import { DeviceDialogData } from '../../devices/device-dialog/device-dialog-data';
import { ScheduleDevice } from 'src/app/shared/models/schedule-device';
import { Device } from 'src/app/shared/models/device';

@Component({
  selector: 'app-schedule-device',
  templateUrl: './schedule-device.component.html',
  styleUrls: ['./schedule-device.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ScheduleDeviceComponent {
  @Input() public scheduleDevice: ScheduleDevice;

  constructor(
    private _schedulesService: SchedulesService,
    private _dialog: MatDialog,
    private _cdr: ChangeDetectorRef
  ) {}

  public saveChanges(newData: string) {
    this._schedulesService.addOrUpdateScheduleDevice(
      this.scheduleDevice.scheduleId,
      this.scheduleDevice.deviceId,
      newData
    ).subscribe(() => {
      this.scheduleDevice = {
        ...this.scheduleDevice,
        deviceData: newData
      };
      this._cdr.detectChanges();
    });
  }

  public composeDialog() {
    this._dialog.open(DeviceDialogComponent, {
      data: <DeviceDialogData> {
        device: this.device,
        showSchedules: false
      }
    }).afterClosed()
      .subscribe(result => {
        if(!result) return;

        this.saveChanges(result);
      });
  }

  public get device(): Device {
    return {
      id: this.scheduleDevice.deviceId,
      name: this.scheduleDevice.name,
      data: this.scheduleDevice.deviceData,
      type: this.scheduleDevice.type
    }
  }
}
