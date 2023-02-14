import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { catchError, map, of, startWith, Subject, switchMap, tap } from 'rxjs';
import { Device } from 'src/app/models/device';
import { Widget } from 'src/app/models/widget';
import { WidgetType } from 'src/app/models/widget-type';
import { RefreshService } from 'src/app/services/refresh.service';
import { WidgetsService } from 'src/app/services/widgets.service';

@UntilDestroy()
@Component({
  selector: 'app-widget',
  templateUrl: './widget.component.html',
  styleUrls: ['./widget.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class WidgetComponent implements OnInit {
  @Input() public widget: Widget;

  private devicesSubject$ = new Subject<Device[]>();
  public devices$ = this.devicesSubject$.asObservable();

  constructor(
    private _widgetsService: WidgetsService,
    private _refreshService: RefreshService
  ) { }
  
  ngOnInit(): void {
    this.getDevices();

    this._refreshService.refresh$
      .pipe(untilDestroyed(this))
      .subscribe(_ => this.getDevices())
  }

  public get widgetStyle(): string {
    const additionalStyle = this.widget.showBorder ? ' widget-border' : '';
    return this._widgetsService.resolveWidgetStyle(this.widget) + additionalStyle;
  }

  public get type(): string{
    return WidgetType[this.widget.widgetType];
  }

  public removeWidget(){
    this._widgetsService.removeWidget(this.widget.id)
      .subscribe(_ => this._refreshService.refresh());
  }

  public drop(event: CdkDragDrop<Device>) {
    const device: Device = event.item.data;
    this._widgetsService.insertDevice(this.widget.id, device.id)
      .subscribe(_ => this._refreshService.refresh());
  }

  public getDevices(){
    this._widgetsService.getWidgetDevices(this.widget.id)
      .subscribe(devices => this.devicesSubject$.next(devices));
  }
}
