import { Component, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Subject } from 'rxjs';
import { Schedule } from 'src/app/models/schedule';
import { RefreshService } from 'src/app/services/refresh.service';
import { SchedulesService } from 'src/app/services/schedules.service';

@UntilDestroy()
@Component({
  selector: 'app-schedules',
  templateUrl: './schedules.component.html',
  styleUrls: ['./schedules.component.scss']
})
export class SchedulesComponent implements OnInit {
  public schedulesSubject$ = new Subject<Schedule[]>();
  public schedules$ = this.schedulesSubject$.asObservable();

  constructor(
    private _refreshService: RefreshService,
    private _schedulesService: SchedulesService
  ) { }

  ngOnInit(): void {
    this._refreshService.refresh$
      .pipe(untilDestroyed(this))
      .subscribe(() => this.getSchedules());

    this._refreshService.refresh();
  }

  private getSchedules(){
    this._schedulesService.getSchedules()
      .subscribe(s => this.schedulesSubject$.next(s));
  }
}
