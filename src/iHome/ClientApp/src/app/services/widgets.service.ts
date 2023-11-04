import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { WidgetType } from '../shared/models/widget-type';
import { Widget } from '../shared/models/widget';
import { Device } from '../shared/models/device';

@Injectable({
  providedIn: 'root'
})
export class WidgetsService {

  private readonly _baseApiUrl = `${environment.authAudience}/Widget/`;

  constructor(private _api: HttpClient) { }

  public addWidget(widgetType: WidgetType, roomId: string, showBorder: boolean){
    return this._api.post(this._baseApiUrl + 'AddWidget', {
      widgetType,
      roomId,
      showBorder
    });
  }

  public insertDevice(widgetId: string, deviceId: string): Observable<object>{
    return this._api.post<object>(this._baseApiUrl + 'InsertDevice', {
      widgetId,
      deviceId
    });
  }

  public removeDevice(widgetId: string, deviceId: string): Observable<object> {
    return this._api.post(this._baseApiUrl + 'RemoveDevice', {
      widgetId,
      deviceId
    });
  }

  public getWidgets(roomId: string): Observable<Widget[]> {
    return this._api.get<Widget[]>(this._baseApiUrl + 'GetWidgets/' + roomId);
  }

  public getWidgetDevices(widgetId: string): Observable<Device[]> {
    return this._api.get<Device[]>(this._baseApiUrl + 'GetWidgetDevices/' + widgetId);
  }

  public removeWidget(widgetId: string): Observable<object> {
    return this._api.delete(this._baseApiUrl + 'RemoveWidget/' + widgetId);
  }

  public resolveWidgetStyle(widget: Widget): string{
    return this.resolveWidgetStyleByType(widget.widgetType);
  }

  public resolveWidgetStyleByType(widgetType: WidgetType): string{
    switch(widgetType) {
      case WidgetType.Small:
        return "small-widget";
      case WidgetType.Medium:
        return "medium-widget";
      case WidgetType.Wide:
        return "wide-widget";
      default:
        return "";
    }
  }
}
