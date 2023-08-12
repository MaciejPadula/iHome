import { Component, Inject, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { debounceTime, filter, Subject } from 'rxjs';
import { Room } from 'src/app/models/room';
import { User } from 'src/app/models/user';
import { RefreshService } from 'src/app/services/refresh.service';
import { SharingService } from 'src/app/services/sharing.service';
import { UsersService } from 'src/app/services/users.service';

@UntilDestroy()
@Component({
  selector: 'app-share-room-dialog',
  templateUrl: './share-room-dialog.component.html',
  styleUrls: ['./share-room-dialog.component.scss']
})
export class ShareRoomDialogComponent implements OnInit {
  public searchPhrase = new FormControl('');
  public selectedUser: User | null;

  private filteredUsersSubject$ = new Subject<User[]>();
  public filteredUsers$ = this.filteredUsersSubject$.asObservable();

  private usersSubject$ = new Subject<User[]>();
  public users$ = this.usersSubject$.asObservable();

  private readonly debounceDelay = 500;

  constructor(
    public dialogRef: MatDialogRef<ShareRoomDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public room: Room,
    private _sharingService: SharingService,
    private _usersService: UsersService,
    private _refreshService: RefreshService
  ) {}
  ngOnInit(): void {
    this._refreshService.refreshRoomUsers$
      .pipe(
        untilDestroyed(this),
        filter(r => r == this.room.id)
      )
      .subscribe(() => this.updateUsers());

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
        this._usersService.getUsers((value as any).name ?? value)
          .subscribe(usrs => this.filteredUsersSubject$.next(usrs));
      });

    this.updateUsers();
  }

  public onNoClick(): void {
    this.dialogRef.close();
  }

  public share(){
    if(!this.selectedUser) return;
    this.searchPhrase.setValue('');

    this._sharingService.shareRoom(this.room.id, this.selectedUser.id)
      .subscribe(() => this._refreshService.refreshRoomUsers(this.room.id));
  }

  public unshare(user: User){
    this._sharingService.unshareRoom(this.room.id, user.id)
      .subscribe(() => this._refreshService.refreshRoomUsers(this.room.id));
  }

  public selectionChange(select: User){
    this.selectedUser = select;
  }

  public userMapper = (user: User) => {
    return user.name;
  };

  private updateUsers(){
    this._usersService.getRoomUsers(this.room.id)
      .subscribe(users => this.usersSubject$.next(users));
  }
}
