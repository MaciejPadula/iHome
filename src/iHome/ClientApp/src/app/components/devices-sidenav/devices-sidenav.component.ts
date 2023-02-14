import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Subject } from 'rxjs';
import { Device } from 'src/app/models/device';
import { DevicesService } from 'src/app/services/devices.service';

@Component({
  selector: 'app-devices-sidenav',
  templateUrl: './devices-sidenav.component.html',
  styleUrls: ['./devices-sidenav.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DevicesSidenavComponent {
  @Input() public roomId: string;
  public opened = false;

  private devicesSubject$ = new Subject<Device[]>();
  public devices$ = this.devicesSubject$.asObservable();

  constructor(
    private _devicesService: DevicesService
  ) { }

  public toggle() {
    if(!this.opened){
      this._devicesService.getRoomDevices(this.roomId)
        .subscribe(resp => this.devicesSubject$.next(resp));
    }

    this.opened = !this.opened;
  }

}
