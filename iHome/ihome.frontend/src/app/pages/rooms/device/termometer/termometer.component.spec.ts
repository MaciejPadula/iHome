import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TermometerComponent } from './termometer.component';

describe('TermometerComponent', () => {
  let component: TermometerComponent;
  let fixture: ComponentFixture<TermometerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TermometerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TermometerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
