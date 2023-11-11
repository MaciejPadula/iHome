import { ChangeDetectionStrategy, Component, Input, OnInit, signal } from '@angular/core';
import { UntilDestroy } from '@ngneat/until-destroy';
import { SchedulesService } from 'src/app/services/data/schedules.service';
import { TimeHelper } from 'src/app/shared/helpers/time.helper';
import { Device } from 'src/app/shared/models/device';
import { DeviceType } from 'src/app/shared/models/device-type';
import { Schedule } from 'src/app/shared/models/schedule';
import { ScheduleDevice } from 'src/app/shared/models/schedule-device';

@UntilDestroy()
@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ScheduleComponent implements OnInit {
  @Input() public schedule: Schedule;
  @Input() public devicesForScheduling: Device[] | null;

  public scheduleHour = "";
  public scheduleDevices = signal<ScheduleDevice[]>([]);
  public isLoading = signal<boolean>(false);

  constructor(
    private _schedulesService: SchedulesService,
    private _timeHelper: TimeHelper
  ) {}

  ngOnInit(): void {
    this.schedule.minute = this.schedule.minute % 5 == 0 ? this.schedule.minute : this.schedule.minute * 5 % 60;
    this.scheduleHour = this._timeHelper.getLocalDateFromUTC(this.schedule.hour, this.schedule.minute);
    this.isLoading.set(true);
    this._schedulesService.getScheduleDevices(this.schedule.id)
      .subscribe(d => {
        this.scheduleDevices.set(d);
        this.isLoading.set(false);
      });
  }

  public addDeviceSnapshot(device: Device) {
    this.isLoading.set(true);
    this._schedulesService.addOrUpdateScheduleDevice(
      this.schedule.id,
      device.id,
      device.data ?? '{}'
    ).subscribe(response => {
      this.scheduleDevices.update(d => [
        ...d.filter(dev => dev.id != response.scheduleDeviceId),
        <ScheduleDevice> {
          id: response.scheduleDeviceId,
          name: device.name,
          deviceId: device.id,
          deviceData: device.data,
          type: device.type,
          scheduleId: this.schedule.id
        }
      ]);
      this.isLoading.set(false);
    });
  }

  public hourChanged(event: string) {
    const dateString = this._timeHelper.getUtcDateStringFromLocalTimeString(event);
    const time = this._timeHelper.getDateFromTimeString(dateString);

    this.schedule.hour = time.getHours();
    this.schedule.minute = time.getMinutes();
    this._schedulesService.updateSchedule(this.schedule.id, dateString).subscribe();
  }
}
