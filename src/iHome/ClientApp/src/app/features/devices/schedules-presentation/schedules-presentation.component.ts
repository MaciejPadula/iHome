import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { DevicesService } from 'src/app/services/devices.service';
import { TimeHelper } from 'src/app/shared/helpers/time.helper';
import { Schedule } from 'src/app/shared/models/schedule';

@Component({
  selector: 'app-schedules-presentation',
  templateUrl: './schedules-presentation.component.html',
  styleUrls: ['./schedules-presentation.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SchedulesPresentationComponent implements OnInit {
  @Input() public deviceId: string;

  private schedulesSubject$ = new Subject<Schedule[]>();
  public schedules$ = this.schedulesSubject$.asObservable();

  constructor(
    private _devicesService: DevicesService,
    private _timeHelper: TimeHelper
  ){}
  
  ngOnInit(): void {
    this._devicesService.getSchedules(this.deviceId)
      .subscribe(x => this.schedulesSubject$.next(x));
  }

  public getScheduleTime(schedule: Schedule): string {
    return this._timeHelper.getLocalDateFromUTC(schedule.hour, schedule.minute);
  }
}
