import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { RoomsApiService } from '../rooms-api.service';

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
  constructor(public dialog: MatDialog, private api: RoomsApiService) { }

  openDialog() {
    const dialogRef = this.dialog.open(AddRoomDialogComponent, {
      width: '20rem',
      data: {roomName: "", roomDescription: ""},
    });
    dialogRef.afterClosed().subscribe(result => {
      if(result!=undefined){
        if(result.roomName.length>3){
          this.api.addRoom(result.roomName, result.roomDescription).subscribe();
        }
      }
    });
  }
}

@Component({
  selector: 'add-room-dialog',
  templateUrl: './add-room-dialog.component.html',
})
export class AddRoomDialogComponent {
  constructor(public dialogRef: MatDialogRef<AddRoomDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: RoomData,){}
  onNoClick(): void {
    this.dialogRef.close();
  }
}
