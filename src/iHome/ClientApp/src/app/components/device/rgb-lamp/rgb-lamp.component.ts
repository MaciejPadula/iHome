import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { filter, map, Observable, Subject } from 'rxjs';
import { DeviceDataHelper } from 'src/app/helpers/device-data.helper';
import { Device } from 'src/app/models/device';
import { DevicesService } from 'src/app/services/devices.service';
import { RefreshService } from 'src/app/services/refresh.service';
import { RgbLampData } from './rgb-lamp-data';
import { RgbLampDialogComponent } from './rgb-lamp-dialog/rgb-lamp-dialog.component';

@UntilDestroy()
@Component({
  selector: 'app-rgb-lamp',
  templateUrl: './rgb-lamp.component.html',
  styleUrls: ['./rgb-lamp.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RgbLampComponent implements OnInit {
  @Input() public device: Device;

  public stateControl = new FormControl(false);

  private dataSubject$ = new Subject<RgbLampData>();
  public data$: Observable<RgbLampData>;

  private readonly defaultData: RgbLampData = {
    red: 0,
    green: 0,
    blue: 0,
    state: false,
    mode: 0
  }

  constructor(
    private _deviceService: DevicesService,
    private _dialog: MatDialog,
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

    this.data$ = this.dataSubject$.asObservable()
      .pipe(
        map(data => {
          this.stateControl.setValue(data.state);
          return data;
        })
      )

    this._refreshService.refreshDevice(this.device.id);
  }

  public updateDeviceData(currentData: RgbLampData){
    const data = this._deviceDataHelper
      .getRgbLampDataWithAndOverrideState(currentData, this.stateControl.value);

    const json = JSON.stringify(data);

    this._deviceService.setDeviceData(this.device.id, json)
      .subscribe(() => this._refreshService.refreshDevice(this.device.id));
  }

  public getColorHex(currentData: RgbLampData): string {
    return this._deviceDataHelper.getColorHexWithState(currentData);
  }

  private getDeviceData() {
    this._deviceService.getDeviceData<RgbLampData>(this.device.id)
      .subscribe(data => this.dataSubject$.next(data ?? this.defaultData));
  }

  public composeDialog(data: RgbLampData){
    this._dialog.open(RgbLampDialogComponent, {
      width: '350px',
      data: {
        name: this.device.name,
        data
      }
    })
    .afterClosed()
    .subscribe(result => {
      if(!result) return;

      this.updateDeviceData(result)
    });
  }
}
