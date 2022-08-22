import { Component, Inject, Input, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { RoomsApiService } from '../rooms-api.service';

@Component({
  selector: 'app-remove-room',
  templateUrl: './remove-room.component.html',
  styleUrls: ['./remove-room.component.scss']
})
export class RemoveRoomComponent implements OnInit {
  @Input() roomId: string = "0";
  @Input() roomName: string = "";
  constructor(public dialog: MatDialog, private api: RoomsApiService) { }

  ngOnInit(): void {
  }
  openDialog() {
    const dialogRef = this.dialog.open(RemoveRoomDialogComponent, {
      width: '20rem',
      data:this.roomName
    });
    dialogRef.afterClosed().subscribe(result => {
      if(result){
        this.api.removeRoom(parseInt(this.roomId)).subscribe();
      }
    });
  }
  
} 

@Component({
  selector: 'remove-room-dialog',
  templateUrl: './remove-room-dialog.component.html',
})
export class RemoveRoomDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<RemoveRoomDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: string,
  ) {}
};