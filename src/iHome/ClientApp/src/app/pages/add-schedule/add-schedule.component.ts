import { ChangeDetectionStrategy, Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TimeHelper } from 'src/app/helpers/time.helper';
import { SchedulesService } from 'src/app/services/schedules.service';

@Component({
  selector: 'app-add-schedule',
  templateUrl: './add-schedule.component.html',
  styleUrls: ['./add-schedule.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddScheduleComponent {
  addScheduleStepperFirstFormGroup = this._formBuilder.group({
    scheduleName: ['', [Validators.required, Validators.minLength(3)]],
  });
  addScheduleStepperSecondFormGroup = this._formBuilder.group({
    scheduleTime: [this.todayDate, Validators.required],
  });

  constructor(
    private _schedulesService: SchedulesService,
    private _timeHelper: TimeHelper,
    private _formBuilder: FormBuilder,
    private _router: Router
  ) {}

  public get todayDate(){
    const date = new Date();

    return this._timeHelper.timeFormatPipe(date.getHours(), date.getMinutes())
  }

  public addSchedule() {
    const name = this.addScheduleStepperFirstFormGroup.value.scheduleName;
    const dateString = this.addScheduleStepperSecondFormGroup.value.scheduleTime;

    if(!name || !dateString) return;

    const date = this._timeHelper.getDateFromTimeString(dateString);

    this._schedulesService.addSchedule(name, date.getUTCHours(), date.getUTCMinutes())
        .subscribe(() => this._router.navigate(['/schedules']));
  }
}
