import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RenameRoomDialogComponent } from './rename-room-dialog.component';

describe('RenameRoomDialogComponent', () => {
  let component: RenameRoomDialogComponent;
  let fixture: ComponentFixture<RenameRoomDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RenameRoomDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RenameRoomDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
