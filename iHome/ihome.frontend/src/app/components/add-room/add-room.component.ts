import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { RoomsApiService } from '../../services/rooms-api.service';

export interface RoomData {
  roomName: string;
  roomDescription: string;
}

@Component({
  selector: 'add-room',
  templateUrl: './add-room.component.html',
  styleUrls: ['./add-room.component.scss']
})
export class AddRoomComponent {
  constructor(public dialog: MatDialog) { }

  openDialog() {
    const dialogRef = this.dialog.open(AddRoomDialogComponent, {
      width: '20rem',
    });
    dialogRef.afterClosed().subscribe();
  }
}

@Component({
  selector: 'add-room-dialog',
  templateUrl: './add-room-dialog.component.html',
})
export class AddRoomDialogComponent {
  roomName: string = "";
  roomDescription: string = "";

  constructor(public dialogRef: MatDialogRef<AddRoomDialogComponent>, private api: RoomsApiService) { }

  onNoClick(): void {
    this.dialogRef.close();
  }
  
  addRoom(): void {
    if(this.roomName.length > 3){
      this.api.addRoom(this.roomName, this.roomDescription).subscribe(_ => this.dialogRef.close());
    }
  }
}
