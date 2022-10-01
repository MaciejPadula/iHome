import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IndexButtonComponent } from './index-button.component';

describe('IndexButtonComponent', () => {
  let component: IndexButtonComponent;
  let fixture: ComponentFixture<IndexButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ IndexButtonComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IndexButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
