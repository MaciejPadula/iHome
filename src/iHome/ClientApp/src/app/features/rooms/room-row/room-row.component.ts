import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Room } from 'src/app/shared/models/room';
import { ShareRoomDialogComponent } from 'src/app/features/rooms/share-room-dialog/share-room-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { AuthService, User } from '@auth0/auth0-angular';
import { RoomsBehaviourService } from 'src/app/pages/rooms/service/rooms-behaviour.service';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { ConfirmDialogData } from 'src/app/shared/components/confirm-dialog/confirm-dialog-data';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-room-row',
  templateUrl: './room-row.component.html',
  styleUrl: './room-row.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RoomRowComponent {
  @Input() public room: Room;

  constructor(
    private _dialog: MatDialog,
    private _auth: AuthService,
    private _roomsBehaviour: RoomsBehaviourService
  ) { }

  public composeShareRoomDialog(room: Room) {
    this._dialog.open(ShareRoomDialogComponent, {
      data: room,
      width: '600px'
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
}
