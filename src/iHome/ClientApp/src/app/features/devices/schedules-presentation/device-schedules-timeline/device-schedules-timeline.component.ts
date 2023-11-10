import { ChangeDetectionStrategy, Component, Input, signal } from '@angular/core';
import { DevicesService } from 'src/app/services/devices.service';
import { Schedule } from 'src/app/shared/models/schedule';

@Component({
  selector: 'app-device-schedules-timeline',
  templateUrl: './device-schedules-timeline.component.html',
  styleUrl: './device-schedules-timeline.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DeviceSchedulesTimelineComponent {
  @Input() public deviceId: string;
  public schedules = signal<Schedule[]>([]);

  constructor(
    private _devicesService: DevicesService
  ){ }

  ngOnInit(): void {
    this._devicesService.getSchedules(this.deviceId)
      .subscribe(x => this.schedules.set(x));
  }
}
