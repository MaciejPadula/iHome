import { Injectable, signal } from '@angular/core';
import { Subject } from 'rxjs';
import { RoomsService } from 'src/app/services/data/rooms.service';
import { Room } from 'src/app/shared/models/room';

@Injectable({
  providedIn: 'root'
})
export class RoomsBehaviourService {
  private roomsSubject$ = new Subject<Room[]>();
  public rooms$ = this.roomsSubject$.asObservable();

  private _isLoading = signal<boolean>(false);
  public isLoading = this._isLoading.asReadonly();

  constructor(
    private _roomsService: RoomsService
  ) { }
  
  public addRoom(roomName: string) {
    if(roomName.length <= 3) return;

    this._isLoading.set(true);
    this._roomsService.addRoom(roomName)
      .subscribe(() => this.getRooms());
  }

  public getRooms() {
    this._isLoading.set(true);
    this._roomsService.getRooms()
      .subscribe(rooms => {
        this.roomsSubject$.next(rooms);
        this._isLoading.set(false);
      });
  }

  public removeRoom(roomId: string) {
    this._isLoading.set(true);
    this._roomsService.removeRoom(roomId)
      .subscribe(() => this.getRooms());
  }
}
