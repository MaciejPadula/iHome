import { HttpClient, HttpHandler } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { Widget } from '../models/widget';
import { WidgetType } from '../models/widget-type';

import { WidgetsService } from './widgets.service';

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
      roomId: ''
    }

    //Act
    const widgetClass = service.resolveWidgetStyle(widget)

    //Assert
    expect(widgetClass).toBe('small-widget');
  });
});
