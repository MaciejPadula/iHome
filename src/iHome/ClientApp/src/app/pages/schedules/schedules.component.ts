import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TimeHelper } from 'src/app/helpers/time.helper';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { ConfirmDialogData } from 'src/app/shared/components/confirm-dialog/confirm-dialog-data';
import { SchedulesBehaviourService } from './service/schedules-behaviour.service';

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

  ngOnInit(): void {
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

  public get devicesForScheduling$() {
    return this._schedulesBehaviour.devicesForScheduling$;
  }

  public get schedules$() {
    return this._schedulesBehaviour.schedules$;
  }
}
