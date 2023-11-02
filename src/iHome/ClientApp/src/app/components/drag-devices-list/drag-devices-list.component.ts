import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { DevicesService } from 'src/app/services/devices.service';
import { Device } from 'src/app/shared/models/device';

@Component({
  selector: 'app-drag-devices-list',
  templateUrl: './drag-devices-list.component.html',
  styleUrls: ['./drag-devices-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DragDevicesListComponent implements OnInit {
  @Input() public roomId: string;

  private devicesSubject$ = new Subject<Device[]>();
  public devices$ = this.devicesSubject$.asObservable();

  constructor(
    private _devicesService: DevicesService
  ) { }

  ngOnInit(): void {
    this._devicesService.getRoomDevices(this.roomId)
      .subscribe(resp => this.devicesSubject$.next(resp));
  }
}
