import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ThermometerComponent } from './thermometer.component';

describe('ThermometerComponent', () => {
  let component: ThermometerComponent;
  let fixture: ComponentFixture<ThermometerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ThermometerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ThermometerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
