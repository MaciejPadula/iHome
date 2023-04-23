import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Subject } from 'rxjs';
import { InputDialogData } from 'src/app/components/input-dialog/input-dialog-data';
import { InputDialogComponent } from 'src/app/components/input-dialog/input-dialog.component';
import { Schedule } from 'src/app/models/schedule';
import { RefreshService } from 'src/app/services/refresh.service';
import { SchedulesService } from 'src/app/services/schedules.service';

@UntilDestroy()
@Component({
  selector: 'app-schedules',
  templateUrl: './schedules.component.html',
  styleUrls: ['./schedules.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SchedulesComponent implements OnInit {
  public schedulesSubject$ = new Subject<Schedule[]>();
  public schedules$ = this.schedulesSubject$.asObservable();

  constructor(
    private _refreshService: RefreshService,
    private _schedulesService: SchedulesService,
    private _dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this._refreshService.refresh$
      .pipe(untilDestroyed(this))
      .subscribe(() => this.getSchedules());

    this._refreshService.refresh();
  }

  public composeDialogAddSchedule() {
    this._dialog.open(InputDialogComponent, {
      data: <InputDialogData> {
        title: 'Add Schedule',
        inputText: 'Schedule name'
      }
    })
    .afterClosed()
    .subscribe(result => {
      if(!result) return;

      const date = new Date();
      this._schedulesService.addSchedule(result, date.getHours(), date.getMinutes())
        .subscribe(() => this._refreshService.refresh());
    });
  }

  private getSchedules() {
    this._schedulesService.getSchedules()
      .subscribe(s => this.schedulesSubject$.next(s));
  }
}
