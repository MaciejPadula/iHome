import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RgbLampComponent } from './rgb-lamp.component';

describe('RgbLampComponent', () => {
  let component: RgbLampComponent;
  let fixture: ComponentFixture<RgbLampComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RgbLampComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RgbLampComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
