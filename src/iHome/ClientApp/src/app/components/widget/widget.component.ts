import { Component, Input } from '@angular/core';
import { Widget } from 'src/app/models/widget';
import { WidgetType } from 'src/app/models/widget-type';
import { RefreshService } from 'src/app/services/refresh.service';
import { WidgetsService } from 'src/app/services/widgets.service';

@Component({
  selector: 'app-widget',
  templateUrl: './widget.component.html',
  styleUrls: ['./widget.component.scss']
})
export class WidgetComponent {
  @Input() public widget: Widget;

  constructor(
    private _widgetsService: WidgetsService,
    private _refreshService: RefreshService
  ) { }

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

}
