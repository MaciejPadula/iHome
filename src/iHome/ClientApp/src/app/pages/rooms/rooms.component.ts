import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService, User } from '@auth0/auth0-angular';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Observable, Subject } from 'rxjs';
import { AddRoomDialogComponent } from 'src/app/components/add-room-dialog/add-room-dialog.component';
import { ShareRoomDialogComponent } from 'src/app/components/share-room-dialog/share-room-dialog.component';
import { Room } from 'src/app/models/room';
import { RefreshService } from 'src/app/services/refresh.service';
import { RoomsService } from 'src/app/services/rooms.service';

@UntilDestroy()
@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrls: ['./rooms.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RoomsComponent implements OnInit {
  private roomsSubject$ = new Subject<Room[]>();
  public rooms$ = this.roomsSubject$.asObservable();

  public anyRoom = false;

  constructor(
    private _roomsService: RoomsService,
    private _refreshService: RefreshService,
    private _router: Router,
    private _route: ActivatedRoute,
    private _dialog: MatDialog,
    private _auth: AuthService
  ) { }

  ngOnInit(): void {
    this._refreshService.refresh$
      .pipe(untilDestroyed(this))
      .subscribe(() => this.loadRooms());

    this._route.params
      .pipe(untilDestroyed(this))
      .subscribe(params => {
        if(params['id']) {
          this.anyRoom = true;
          return;
        }
        this.anyRoom = false;
        this._refreshService.refresh();
      });

    this._refreshService.refresh();
  }

  private loadRooms(){
    this._roomsService.getRooms()
      .subscribe(rooms => this.roomsSubject$.next(rooms));
  }

  public composeAddRoomDialog(){
    this._dialog.open(AddRoomDialogComponent)
      .afterClosed()
      .subscribe(data => {
        if(!data) return;
        if(data.roomName.length <= 3) return;

        this._roomsService.addRoom(data.roomName)
          .subscribe(() => this._refreshService.refresh());
      });
  }

  public composeShareRoomDialog(room: Room) {
    this._dialog.open(ShareRoomDialogComponent, {
      data: room
    })
      .afterClosed()
      .subscribe();
  }

  public removeRoom(roomId: string){
    this._roomsService.removeRoom(roomId)
      .subscribe(() => this._router.navigate(['/rooms']));
  }

  public get user(): Observable<User | null | undefined> {
    return this._auth.user$;
  }
}
