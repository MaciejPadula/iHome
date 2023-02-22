import { Component, Inject, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { debounceTime, Subject } from 'rxjs';
import { Room } from 'src/app/models/room';
import { User } from 'src/app/models/user';
import { RoomsService } from 'src/app/services/rooms.service';
import { UsersService } from 'src/app/services/users.service';

@UntilDestroy()
@Component({
  selector: 'app-share-room-dialog',
  templateUrl: './share-room-dialog.component.html',
  styleUrls: ['./share-room-dialog.component.scss']
})
export class ShareRoomDialogComponent implements OnInit {
  public searchPhrase = new FormControl('');

  private filteredUsersSubject$ = new Subject<User[]>();
  public filteredUsers$ = this.filteredUsersSubject$.asObservable();

  private readonly debounceDelay = 500;

  constructor(
    public dialogRef: MatDialogRef<ShareRoomDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public room: Room,
    private _roomsService: RoomsService,
    private _usersService: UsersService
  ) {}
  ngOnInit(): void {
    // this._roomsService.getRoomUsers(this.room.id)
    //   .subscribe(u => console.log(u));

    this.searchPhrase.valueChanges
      .pipe(
        untilDestroyed(this),
        debounceTime(this.debounceDelay)
      )
      .subscribe(value => {
        if(!value || value.length < 3) {
          this.filteredUsersSubject$.next([]);
          return;
        }
        this._usersService.getUsers(value)
          .subscribe(usrs => this.filteredUsersSubject$.next(usrs));
      });
  }

  public onNoClick(): void {
    this.dialogRef.close();
  }
}
