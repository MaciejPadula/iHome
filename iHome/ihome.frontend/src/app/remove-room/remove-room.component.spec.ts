import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RemoveRoomComponent } from './remove-room.component';

describe('RemoveRoomComponent', () => {
  let component: RemoveRoomComponent;
  let fixture: ComponentFixture<RemoveRoomComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RemoveRoomComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RemoveRoomComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
