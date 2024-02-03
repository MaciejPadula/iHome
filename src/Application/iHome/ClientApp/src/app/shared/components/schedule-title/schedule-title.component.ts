import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Schedule } from 'src/app/shared/models/schedule';

@Component({
  selector: 'app-schedule-title',
  templateUrl: './schedule-title.component.html',
  styleUrls: ['./schedule-title.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ScheduleTitleComponent {
  @Input() public schedule: Schedule;
  @Input() public time: string | null = null;

  
  public get showTime() : boolean {
    return this.time != null;
  }
  
}
