import { ChangeDetectionStrategy, Component, Input, OnInit, signal } from '@angular/core';
import { DevicesService } from 'src/app/services/data/devices.service';
import { Schedule } from 'src/app/shared/models/schedule';

@Component({
  selector: 'app-device-schedules-timeline',
  templateUrl: './device-schedules-timeline.component.html',
  styleUrl: './device-schedules-timeline.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DeviceSchedulesTimelineComponent implements OnInit {
  @Input() public deviceId: string;
  public schedules = signal<Schedule[]>([]);
  public isLoading = signal<boolean>(false);

  constructor(
    private _devicesService: DevicesService
  ){ }

  ngOnInit(): void {
    this.isLoading.set(true);
    this._devicesService.getSchedules(this.deviceId)
      .subscribe(x => {
        this.schedules.set(x);
        this.isLoading.set(false);
      });
  }
}
