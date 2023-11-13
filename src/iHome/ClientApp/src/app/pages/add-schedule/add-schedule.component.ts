import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { DevicesService } from 'src/app/services/data/devices.service';
import { SchedulesService } from 'src/app/services/data/schedules.service';
import { SuggestionsService } from 'src/app/services/data/suggestions.service';
import { TimeHelper } from 'src/app/shared/helpers/time.helper';
import { Device } from 'src/app/shared/models/device';

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

  public devicesForSchedulingSubject$ = new Subject<Device[]>();
  public devicesForScheduling$ = this.devicesForSchedulingSubject$.asObservable();

  public suggestedDevicesSubject$ = new Subject<string[]>();
  public suggestedDevices$ = this.suggestedDevicesSubject$.asObservable();

  loadingSuggestions = false;

  private readonly timeStepIndex = 1;
  private readonly devicesStepIndex = 2;

  constructor(
    private _suggestionsService: SuggestionsService,
    private _schedulesService: SchedulesService,
    private _devicesService: DevicesService,
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
    const dateString = this._timeHelper.getUtcDateStringFromLocalTimeString(
      this.addScheduleStepperSecondFormGroup.value.scheduleTime ?? '0:0'
    );

    if(!name || !dateString) return;

    this._schedulesService.addSchedule(name, dateString)
        .subscribe(() => this._router.navigate(['/schedules']));
  }

  public onTimeStep(event: StepperSelectionEvent) {
    switch(event.selectedIndex) {
      case this.timeStepIndex:
        this.timeStep();
        break;
      case this.devicesStepIndex:
        this.devicesStep();
        break;
    }
  }

  public isSuggested(suggestedDevices: string[], deviceId: string){
    return suggestedDevices.some(d => d == deviceId);
  }

  private get scheduleName() {
    const name = this.addScheduleStepperFirstFormGroup.controls.scheduleName.value;
    return name;
  }

  private timeStep() {
    const name = this.scheduleName;
    if(name == null) return;

    this.loadingSuggestions = true;
    this._suggestionsService.getSuggestedHour(name)
      .subscribe(time => {
        this.addScheduleStepperSecondFormGroup.controls.scheduleTime
          .setValue(this._timeHelper.timeFormatPipe(time.hour, time.minute));
        this.loadingSuggestions = false;
      });
  }

  private devicesStep() {
    const name = this.scheduleName;
    if(name == null) return;

    this._devicesService.getDevicesForScheduling()
      .subscribe(d => this.devicesForSchedulingSubject$.next(d));

    this._suggestionsService.getSuggestedDevices(name, this.addScheduleStepperSecondFormGroup.controls.scheduleTime.value ?? this.todayDate)
      .subscribe(sugg => this.suggestedDevicesSubject$.next(sugg));
  }
}
