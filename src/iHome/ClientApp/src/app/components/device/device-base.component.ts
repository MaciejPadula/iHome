import { Component, Input, OnInit } from "@angular/core";
import { Observable, Subject } from "rxjs";
import { Device } from "src/app/models/device";
import { DevicesService } from "src/app/services/devices.service";

@Component({ template: '' })
export abstract class DeviceBaseComponent<DeviceDataType> implements OnInit {
  @Input() public device: Device;
  private dataSubject$ = new Subject<DeviceDataType>();
  public data$: Observable<DeviceDataType> = this.dataSubject$.asObservable();

  constructor(
    private _devicesService: DevicesService
  ) { }

  ngOnInit(): void {
    this.readData();
  }

  protected readData() {
    this._devicesService.getDeviceData<DeviceDataType>(this.device.id)
      .subscribe(data => this.dataSubject$.next(data ?? this.defaultData));
  }

  protected setData(data: DeviceDataType) {
    this._devicesService.setDeviceData(this.device.id, JSON.stringify(data))
      .subscribe(() => this.readData());
  }

  protected abstract get defaultData(): DeviceDataType;
}