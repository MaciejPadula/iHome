import { ChangeDetectionStrategy, Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UntilDestroy } from '@ngneat/until-destroy';
import { Subject } from 'rxjs';
import { SharingService } from 'src/app/services/data/sharing.service';
import { UsersService } from 'src/app/services/data/users.service';
import { Room } from 'src/app/shared/models/room';
import { User } from 'src/app/shared/models/user';

@UntilDestroy()
@Component({
  selector: 'app-share-room-dialog',
  templateUrl: './share-room-dialog.component.html',
  styleUrls: ['./share-room-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ShareRoomDialogComponent implements OnInit {
  private usersSubject$ = new Subject<User[]>();
  public users$ = this.usersSubject$.asObservable();

  constructor(
    public dialogRef: MatDialogRef<ShareRoomDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public room: Room,
    private _sharingService: SharingService,
    private _usersService: UsersService
  ) {}
  ngOnInit(): void {
    this._usersService.getRoomUsers(this.room.id)
      .subscribe(users => this.usersSubject$.next(users));
  }

  public onNoClick(): void {
    this.dialogRef.close();
  }

  public share(user: User){
    this._sharingService.shareRoom(this.room.id, user.id).subscribe();
  }

  public unshare(user: User){
    this._sharingService.unshareRoom(this.room.id, user.id).subscribe();
  }
}
