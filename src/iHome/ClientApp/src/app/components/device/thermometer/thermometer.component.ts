import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { faThermometerFull } from '@fortawesome/free-solid-svg-icons';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { filter, Subject } from 'rxjs';
import { Device } from 'src/app/models/device';
import { DevicesService } from 'src/app/services/devices.service';
import { RefreshService } from 'src/app/services/refresh.service';
import { ThermometerData } from './thermometer-data';
import { DeviceDataHelper } from 'src/app/helpers/device-data.helper';

@UntilDestroy()
@Component({
  selector: 'app-thermometer',
  templateUrl: './thermometer.component.html',
  styleUrls: ['./thermometer.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ThermometerComponent implements OnInit {
  @Input() public device: Device;
  public faThermometer = faThermometerFull;

  private dataSubject$ = new Subject<ThermometerData>();
  public data$ = this.dataSubject$.asObservable();

  constructor(
    private _deviceService: DevicesService,
    private _refreshService: RefreshService,
    private _deviceDataHelper: DeviceDataHelper
  ) { }

  public ngOnInit(): void {
    this._refreshService.refreshDevice$
      .pipe(
        untilDestroyed(this),
        filter(data => data == this.device.id)
      )
      .subscribe(() => this.getDeviceData());
    
    this._refreshService.refreshDevice(this.device.id);
  }

  private getDeviceData() {
    this._deviceService.getDeviceData<ThermometerData>(this.device.id)
      .subscribe(data => this.dataSubject$.next(data));
  }

  public formattedTemperature(temp: number): string {
    return this._deviceDataHelper.formattedTemperature(temp);
  }

  public formattedPreassure(press: number): string {
    return this._deviceDataHelper.formattedPreassure(press);
  }
}
