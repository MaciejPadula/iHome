import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AvailableDeviceComponent } from './available-device.component';

describe('AvailableDeviceComponent', () => {
  let component: AvailableDeviceComponent;
  let fixture: ComponentFixture<AvailableDeviceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AvailableDeviceComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AvailableDeviceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
