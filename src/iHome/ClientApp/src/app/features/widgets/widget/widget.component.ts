import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { ChangeDetectionStrategy, Component, Input, OnInit, signal } from '@angular/core';
import { UntilDestroy } from '@ngneat/until-destroy';
import { RoomBehaviourService } from 'src/app/pages/room/service/room-behaviour.service';
import { WidgetsService } from 'src/app/services/data/widgets.service';
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
  
  public devices = signal<Device[]>([]);
  public isLoading = signal<boolean>(false);

  constructor(
    private _roomBehaviourService: RoomBehaviourService,
    private _widgetsService: WidgetsService
  ) { }
  
  ngOnInit(): void {
    this.getDevices();
  }

  public get widgetStyle(): string {
    const additionalStyle = this.widget.showBorder ? ' widget-border' : '';
    return this._widgetsService.resolveWidgetStyle(this.widget) + additionalStyle;
  }

  public get type(): string{
    return WidgetType[this.widget.widgetType];
  }

  public removeWidget(){
    this._roomBehaviourService.removeWidget(this.widget.id);
  }

  public drop(event: CdkDragDrop<Device>) {
    const device: Device = event.item.data;
    this._widgetsService.insertDevice(this.widget.id, device.id)
      .subscribe(() => this.devices.update(d => [
        ...d,
        device
      ]));
  }

  public getDevices(){
    this.isLoading.set(true);
    this._widgetsService.getWidgetDevices(this.widget.id)
      .subscribe(devices => {
        this.devices.set(devices);
        this.isLoading.set(false);
      });
  }
}
