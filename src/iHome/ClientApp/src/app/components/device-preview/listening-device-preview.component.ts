import { Component, Input, OnInit } from "@angular/core";
import { Observable, Subject } from "rxjs";
import { Device } from "src/app/models/device";
import { DevicesService } from "src/app/services/devices.service";

@Component({ template: '' })
export abstract class ListeningDevicePreviewComponent<DeviceDataType> implements OnInit {
  @Input() public device: Device;
  private dataSubject$ = new Subject<DeviceDataType>();
  public data$: Observable<DeviceDataType> = this.dataSubject$.asObservable();

  constructor(
    private _devicesService: DevicesService
  ) { }

  ngOnInit(): void {
    this.dataSubject$.next(this.data);
  }

  protected readData() {
    this._devicesService.getDeviceData<DeviceDataType>(this.device.id)
      .subscribe(data => this.dataSubject$.next(data ?? this.defaultData));
  }

  protected get data(): DeviceDataType {
    if (!this.device?.data) {
      return this.defaultData;
    }

    return JSON.parse(this.device.data);
  }

  protected abstract get defaultData(): DeviceDataType;
}