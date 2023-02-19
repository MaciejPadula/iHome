import { Component, Input, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { filter, Subject } from 'rxjs';
import { Device } from 'src/app/models/device';
import { DevicesService } from 'src/app/services/devices.service';
import { RefreshService } from 'src/app/services/refresh.service';
import { ThermometerData } from './thermometer-data';

@UntilDestroy()
@Component({
  selector: 'app-thermometer',
  templateUrl: './thermometer.component.html',
  styleUrls: ['./thermometer.component.scss']
})
export class ThermometerComponent implements OnInit {
  @Input() public device: Device;

  private dataSubject$ = new Subject<ThermometerData>();
  public data$ = this.dataSubject$.asObservable();

  constructor(
    private _deviceService: DevicesService,
    private _refreshService: RefreshService
  ) { }

  public ngOnInit(): void {
    this._refreshService.refreshDevice$
      .pipe(
        untilDestroyed(this),
        filter(data => data == this.device.id)
      )
      .subscribe(_ => this.getDeviceData());
    
    this._refreshService.refreshDevice(this.device.id);
  }

  private getDeviceData() {
    this._deviceService.getDeviceData<ThermometerData>(this.device.id)
      .subscribe(data => this.dataSubject$.next(data));
  }
}
