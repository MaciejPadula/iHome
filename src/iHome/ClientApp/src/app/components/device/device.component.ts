import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Device } from 'src/app/models/device';
import { RefreshService } from 'src/app/services/refresh.service';
import { WidgetsService } from 'src/app/services/widgets.service';
import { DeviceDialogComponent } from '../device-dialog/device-dialog.component';
import { DeviceType } from 'src/app/models/device-type';
import { DevicesService } from 'src/app/services/devices.service';
import { DeviceDialogData } from '../device-dialog/device-dialog-data';

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
    private _widgetsService: WidgetsService,
    private _refreshService: RefreshService,
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

  public removeFromWidget() {
    this._widgetsService.removeDevice(this.widgetId, this.device.id)
      .subscribe(() => this._refreshService.refresh());
  }

  public get showDialog() {
    return this.device.type == DeviceType.RGBLamp;
  }
}
