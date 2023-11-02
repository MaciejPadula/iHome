import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Subject, Subscription } from 'rxjs';
import { RefreshService } from 'src/app/services/refresh.service';
import { WidgetsService } from 'src/app/services/widgets.service';
import { Device } from 'src/app/shared/models/device';
import { Widget } from 'src/app/shared/models/widget';
import { WidgetType } from 'src/app/shared/models/widget-type';

@UntilDestroy()
@Component({
  selector: 'app-widget',
  templateUrl: './widget.component.html',
  styleUrls: ['./widget.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class WidgetComponent implements OnInit {
  @Input() public widget: Widget;

  private refreshSubscription$: Subscription;
  
  private devicesSubject$ = new Subject<Device[]>();
  public devices$ = this.devicesSubject$.asObservable();

  constructor(
    private _widgetsService: WidgetsService,
    private _refreshService: RefreshService
  ) { }
  
  ngOnInit(): void {
    this.getDevices();

    this.refreshSubscription$ = this._refreshService.refresh$
      .pipe(untilDestroyed(this))
      .subscribe(() => this.getDevices())
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
      .subscribe(() => {
        this.refreshSubscription$.unsubscribe();
        this._refreshService.refresh()
      });
  }

  public drop(event: CdkDragDrop<Device>) {
    const device: Device = event.item.data;
    this._widgetsService.insertDevice(this.widget.id, device.id)
      .subscribe(() => this._refreshService.refresh());
  }

  public getDevices(){
    this._widgetsService.getWidgetDevices(this.widget.id)
      .subscribe(devices => this.devicesSubject$.next(devices));
  }
}
