import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DevicesService } from 'src/app/services/data/devices.service';
import { DeviceDialogComponent } from '../device-dialog/device-dialog.component';
import { DeviceDialogData } from '../device-dialog/device-dialog-data';
import { DeviceType } from 'src/app/shared/models/device-type';
import { Device } from 'src/app/shared/models/device';

@Component({
  selector: 'app-device',
  templateUrl: './device.component.html',
  styleUrls: ['./device.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DeviceComponent {
  @Input() public device: Device;
  @Input() public widgetId: string;

  type = DeviceType;

  constructor(
    private _dialog: MatDialog,
    private _devicesService: DevicesService,
    private _cdr: ChangeDetectorRef
  ){}

  public composeDialog(device: Device) {
    this._dialog.open(DeviceDialogComponent, {
      data: <DeviceDialogData> {
        device: device,
        showSchedules: true
      }
    })
      .afterClosed()
      .subscribe(data => {
        if (data != null && data != undefined){
          this._devicesService.setDeviceData(device.id, data)
            .subscribe(() => {
              this.device = {
                ...this.device,
                data
              };
              this._cdr.detectChanges();
            });
        }
      });
  }

  public get showDialog() {
    return this.device.type == DeviceType.RGBLamp;
  }
}
