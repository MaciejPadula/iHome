import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Subject } from 'rxjs';
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

  constructor(
    private _roomsService: RoomsService,
    private _refreshService: RefreshService
  ) { }

  ngOnInit(): void {
    this._refreshService.refresh$
      .pipe(untilDestroyed(this))
      .subscribe(_ => this.loadRooms());

    this._refreshService.refresh();
  }

  private loadRooms(){
    this._roomsService.getRooms()
      .subscribe(rooms => this.roomsSubject$.next(rooms));
  }

  public addRoom(){
    this._roomsService.addRoom("asdasdasdasd")
      .subscribe(_ => this._refreshService.refresh());
  }
}
