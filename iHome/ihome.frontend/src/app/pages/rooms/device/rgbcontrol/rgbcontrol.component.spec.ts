import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RGBControlComponent } from './rgbcontrol.component';

describe('RGBControlComponent', () => {
  let component: RGBControlComponent;
  let fixture: ComponentFixture<RGBControlComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RGBControlComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RGBControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
