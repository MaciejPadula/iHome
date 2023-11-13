import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
  selector: 'app-schedules-presentation',
  templateUrl: './schedules-presentation.component.html',
  styleUrls: ['./schedules-presentation.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SchedulesPresentationComponent {
  @Input() public deviceId: string;
}
