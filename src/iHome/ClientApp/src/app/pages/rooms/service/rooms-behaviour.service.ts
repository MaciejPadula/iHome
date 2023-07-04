import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { Room } from 'src/app/models/room';
import { RefreshService } from 'src/app/services/refresh.service';
import { RoomsService } from 'src/app/services/rooms.service';

@Injectable({
  providedIn: 'root'
})
export class RoomsBehaviourService {
  private roomsSubject$ = new Subject<Room[]>();
  public rooms$ = this.roomsSubject$.asObservable();

  private isLoadingSubject$ = new Subject<boolean>();
  public isLoading$ = this.isLoadingSubject$.asObservable();

  constructor(
    private _roomsService: RoomsService
  ) {
    this.isLoadingSubject$.next(true);
  }
  
  public addRoom(roomName: string) {
    if(roomName.length <= 3) return;

    this.isLoadingSubject$.next(true);
    this._roomsService.addRoom(roomName)
      .subscribe(() => this.getRooms());
  }

  public getRooms() {
    this.isLoadingSubject$.next(true);
    this._roomsService.getRooms()
      .subscribe(rooms => {
        this.roomsSubject$.next(rooms);
        this.isLoadingSubject$.next(false);
      });
  }

  public removeRoom(roomId: string) {
    this.isLoadingSubject$.next(true);
    this._roomsService.removeRoom(roomId)
      .subscribe(() => this.getRooms());
  }
}
