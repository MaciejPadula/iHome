import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Device } from 'src/app/models/device';
import { RefreshService } from 'src/app/services/refresh.service';
import { WidgetsService } from 'src/app/services/widgets.service';
import { DeviceDialogComponent } from '../device-dialog/device-dialog.component';

@Component({
  selector: 'app-device',
  templateUrl: './device.component.html',
  styleUrls: ['./device.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DeviceComponent {
  @Input() public device: Device;
  @Input() public widgetId: string;

  constructor(
    private _dialog: MatDialog,
    private _widgetsService: WidgetsService,
    private _refreshService: RefreshService
  ){}

  public composeDialog(device: Device) {
    this._dialog.open(DeviceDialogComponent, {
      data: device
    })
      .afterClosed()
      .subscribe();
  }

  public removeFromWidget() {
    this._widgetsService.removeDevice(this.widgetId, this.device.id)
      .subscribe(() => this._refreshService.refresh());
  }
}
