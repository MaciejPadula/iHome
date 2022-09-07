import { Component, Inject, Input, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { RoomsApiService } from 'src/app/services/rooms-api.service';

@Component({
  selector: 'app-remove-room',
  templateUrl: './remove-room.component.html',
  styleUrls: ['./remove-room.component.scss']
})
export class RemoveRoomComponent {
  @Input() roomId: string = "0";
  @Input() roomName: string = "";
  constructor(public dialog: MatDialog) { }

  openDialog() {
    const dialogRef = this.dialog.open(RemoveRoomDialogComponent, {
      width: '20rem',
      data: {
        roomId: this.roomId,
        roomName: this.roomName,
      }
    });
    dialogRef.afterClosed().subscribe();
  }
} 

@Component({
  selector: 'remove-room-dialog',
  templateUrl: './remove-room-dialog.component.html',
})
export class RemoveRoomDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<RemoveRoomDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {roomId: number, roomName: string},
    private _api: RoomsApiService
  ) {}

  public async removeRoom(){
    this._api.removeRoom(this.data.roomId);
    this.dialogRef.close();
  }
};