import { ChangeDetectionStrategy, Component, Input, OnInit, signal } from '@angular/core';
import { DevicesService } from 'src/app/services/data/devices.service';
import { Device } from 'src/app/shared/models/device';

@Component({
  selector: 'app-drag-devices-list',
  templateUrl: './drag-devices-list.component.html',
  styleUrls: ['./drag-devices-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DragDevicesListComponent implements OnInit {
  @Input() public roomId: string;

  public devices = signal<Device[]>([]);
  public isLoading = signal<boolean>(false);

  constructor(
    private _devicesService: DevicesService
  ) { }

  ngOnInit(): void {
    this.isLoading.set(true);
    this._devicesService.getRoomDevices(this.roomId)
      .subscribe(resp => {
        this.devices.set(resp);
        this.isLoading.set(false);
      });
  }
}
