import { Injectable, signal } from '@angular/core';
import { WidgetsService } from 'src/app/services/data/widgets.service';
import { Widget } from 'src/app/shared/models/widget';
import { WidgetType } from 'src/app/shared/models/widget-type';

@Injectable({
  providedIn: 'root'
})
export class RoomBehaviourService {
  private _widgets = signal<Widget[]>([]);
  public widgets = this._widgets.asReadonly();

  private _isLoading = signal<boolean>(false);
  public isLoading = this._isLoading.asReadonly();

  constructor(
    private _widgetsService: WidgetsService,
  ) { }

  public addWidget(roomId: string, widgetType: WidgetType, showBorder: boolean) {
    this._isLoading.set(true);
    this._widgetsService.addWidget(widgetType, roomId, showBorder)
      .subscribe(w => {
        this._widgets.set([
          ...this._widgets(),
          <Widget>{
            id: w.widgetId,
            widgetType: widgetType,
            showBorder: showBorder,
            roomId: roomId
          }
        ]);
        this._isLoading.set(false);
      });
  }

  public removeWidget(widgetId: string) {
    this._isLoading.set(true);
    this._widgetsService.removeWidget(widgetId)
      .subscribe(() => {
        this._widgets.update(w => w.filter(x => x.id != widgetId));
        this._isLoading.set(false);
      });
  }

  public getWidgets(roomId: string) {
    this._isLoading.set(true);
    this._widgetsService.getWidgets(roomId)
      .subscribe(widgets => {
        this._widgets.set(widgets);
        this._isLoading.set(false);
      });
  }
}
