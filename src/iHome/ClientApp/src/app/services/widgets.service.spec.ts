import { HttpClient, HttpHandler } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';

import { WidgetsService } from './widgets.service';
import { Widget } from '../shared/models/widget';
import { WidgetType } from '../shared/models/widget-type';

describe('WidgetsService', () => {
  let service: WidgetsService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        HttpClient,
        HttpHandler
      ]
    });
    service = TestBed.inject(WidgetsService);
  });

  it('should return correct widget class', () => {
    //Arrange
    const widget: Widget = {
      id: '',
      widgetType: WidgetType.Small,
      roomId: '',
      showBorder: false
    };

    //Act
    const widgetClass = service.resolveWidgetStyle(widget);

    //Assert
    expect(widgetClass).toBe('small-widget');
  });
});
