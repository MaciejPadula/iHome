import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AuthService, User } from '@auth0/auth0-angular';
import { Observable } from 'rxjs';
import { ShareRoomDialogComponent } from 'src/app/components/share-room-dialog/share-room-dialog.component';
import { Room } from 'src/app/models/room';
import { RoomsBehaviourService } from './service/rooms-behaviour.service';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { ConfirmDialogData } from 'src/app/shared/components/confirm-dialog/confirm-dialog-data';

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrls: ['./rooms.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RoomsComponent implements OnInit {
  constructor(
    private _dialog: MatDialog,
    private _auth: AuthService,
    private _roomsBehaviour: RoomsBehaviourService
  ) { }

  ngOnInit(): void {
    this._roomsBehaviour.getRooms();
  }

  public composeShareRoomDialog(room: Room) {
    this._dialog.open(ShareRoomDialogComponent, {
      data: room
    })
      .afterClosed()
      .subscribe();
  }

  public removeRoom(roomId: string, roomName: string) {
    this._dialog.open(ConfirmDialogComponent, {
      data: <ConfirmDialogData> {
        title: 'Warning',
        additionalText: `You are going to remove room named: ${roomName}. Are you sure?`
      }
    })
    .afterClosed()
    .subscribe(result => {
      if(!result) return;

      this._roomsBehaviour.removeRoom(roomId);
    });
  }

  public get user(): Observable<User | null | undefined> {
    return this._auth.user$;
  }

  public get rooms$() {
    return this._roomsBehaviour.rooms$;
  }

  public get isLoading$() {
    return this._roomsBehaviour.isLoading$;
  }
}
