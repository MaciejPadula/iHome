import { ChangeDetectionStrategy, Component, OnInit, signal } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AuthService, User } from '@auth0/auth0-angular';
import { Observable } from 'rxjs';
import { AddRoomDialogComponent } from 'src/app/features/rooms/add-room-dialog/add-room-dialog.component';
import { RoomsBehaviourService } from './service/rooms-behaviour.service';
import { Room } from 'src/app/shared/models/room';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

@UntilDestroy()
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

  public rooms = signal<Room[]>([]);

  ngOnInit(): void {
    this._roomsBehaviour.rooms$
      .pipe(untilDestroyed(this))
      .subscribe(r => this.rooms.set(r));

    this._roomsBehaviour.getRooms();
  }

  public composeAddRoomDialog(){
    this._dialog.open(AddRoomDialogComponent)
      .afterClosed()
      .subscribe(result => {
        if(!(result?.roomName)) return;

        this._roomsBehaviour.addRoom(result.roomName);
      });
  }

  public get user(): Observable<User | null | undefined> {
    return this._auth.user$;
  }

  public get isLoading() {
    return this._roomsBehaviour.isLoading;
  }
}
