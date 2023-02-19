import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-add-room-dialog',
  templateUrl: './add-room-dialog.component.html',
  styleUrls: ['./add-room-dialog.component.scss']
})
export class AddRoomDialogComponent {
  public roomName: string;

  constructor(
    public dialogRef: MatDialogRef<AddRoomDialogComponent>,
  ) {}

  public confirm(): void{
    if(this.roomName.length <= 3) {
      this.onNoClick();
      return;
    }

    this.dialogRef.close({
      roomName: this.roomName
    });
  }

  onNoClick(): void {
    this.dialogRef.close(null);
  }

}
