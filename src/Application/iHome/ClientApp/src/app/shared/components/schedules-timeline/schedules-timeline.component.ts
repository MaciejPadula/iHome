import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Schedule } from 'src/app/shared/models/schedule';
import { TimeHelper } from 'src/app/shared/helpers/time.helper';

@Component({
  selector: 'app-schedules-timeline',
  templateUrl: './schedules-timeline.component.html',
  styleUrl: './schedules-timeline.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SchedulesTimelineComponent {
  @Input() public schedules: Schedule[];

  constructor(
    private _timeHelper: TimeHelper
  ) { }

  public getScheduleTime(schedule: Schedule): string {
    return this._timeHelper.getLocalDateFromUTC(schedule.hour, schedule.minute);
  }
}
