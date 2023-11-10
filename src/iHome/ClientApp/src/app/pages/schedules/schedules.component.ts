import { ChangeDetectionStrategy, Component, OnInit, signal } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { ConfirmDialogData } from 'src/app/shared/components/confirm-dialog/confirm-dialog-data';
import { SchedulesBehaviourService } from './service/schedules-behaviour.service';
import { TimeHelper } from 'src/app/shared/helpers/time.helper';
import { Subject } from 'rxjs';
import { Device } from 'src/app/shared/models/device';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

@UntilDestroy()
@Component({
  selector: 'app-schedules',
  templateUrl: './schedules.component.html',
  styleUrls: ['./schedules.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SchedulesComponent implements OnInit {
  constructor(
    private _schedulesBehaviour: SchedulesBehaviourService,
    private _timeHelper: TimeHelper,
    private _dialog: MatDialog
  ) { }

  public devicesForScheduling = signal<Device[]>([]);

  ngOnInit(): void {
    this._schedulesBehaviour.devicesForScheduling$
      .pipe(untilDestroyed(this))
      .subscribe(d => this.devicesForScheduling.set(d));

    this._schedulesBehaviour.getSchedules();
  }

  public composeDialogRemoveSchedule(scheduleId: string, scheduleName: string) {
    this._dialog.open(ConfirmDialogComponent, {
      data: <ConfirmDialogData> {
        title: 'Warning',
        additionalText: `You are going to remove schedule named: ${scheduleName}. Are you sure?`
      }
    })
    .afterClosed()
    .subscribe(result => {
      if(!result) return;

      this._schedulesBehaviour.removeSchedule(scheduleId);
    });
  }

  public getTimeFormatted(hour: number, minute: number){
    return this._timeHelper.getLocalDateFromUTC(hour, minute);
  }

  public get isLoading$() {
    return this._schedulesBehaviour.isLoading$;
  }

  public get schedules$() {
    return this._schedulesBehaviour.schedules$;
  }
}
