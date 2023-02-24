import { Component, Input, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Color, ColorPickerControl } from '@iplab/ngx-color-picker';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { debounceTime, filter, map, Observable, Subject } from 'rxjs';
import { Device } from 'src/app/models/device';
import { DevicesService } from 'src/app/services/devices.service';
import { RefreshService } from 'src/app/services/refresh.service';
import { RgbLampData } from './rgb-lamp-data';

@UntilDestroy()
@Component({
  selector: 'app-rgb-lamp',
  templateUrl: './rgb-lamp.component.html',
  styleUrls: ['./rgb-lamp.component.scss']
})
export class RgbLampComponent implements OnInit {
  @Input() public device: Device;

  public colorPickerControl = new ColorPickerControl()
    .hidePresets()
    .hideAlphaChannel();

  public stateControl = new FormControl(false);

  private dataSubject$ = new Subject<RgbLampData>();
  public data$: Observable<RgbLampData>;

  private readonly debounceTime = 700;

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
      .subscribe(() => this.getDeviceData());
    
    this.data$ = this.dataSubject$.asObservable()
      .pipe(
        map(data => {
          const color = new Color().setRgba(data.red, data.green, data.blue);
          this.colorPickerControl.setValueFrom(color);
          this.stateControl.setValue(data.state);
          return data;
        })
      )
   
    this.colorPickerControl.valueChanges
      .pipe(
        untilDestroyed(this),
        debounceTime(this.debounceTime)
      )
      .subscribe(() => this.updateDeviceData())

    this._refreshService.refreshDevice(this.device.id);
  }

  public updateDeviceData(){
    const clr = this.colorPickerControl.value.getRgba();
    const data: RgbLampData = {
      red: Math.round(clr.red),
      green: Math.round(clr.green),
      blue: Math.round(clr.blue),
      state: this.stateControl.value ?? false,
      mode: 0
    }

    this._deviceService.setDeviceData(this.device.id, JSON.stringify(data))
      .subscribe(() => this._refreshService.refreshDevice(this.device.id));
  }

  private getDeviceData() {
    this._deviceService.getDeviceData<RgbLampData>(this.device.id)
      .subscribe(data => this.dataSubject$.next(data));
  }
}
