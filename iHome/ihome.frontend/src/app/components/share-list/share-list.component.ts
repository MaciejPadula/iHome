import { Component, Inject, Input, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { User } from 'src/app/models/user';
import { RoomsApiService } from 'src/app/services/rooms-api.service';

@Component({
  selector: 'app-share-list',
  templateUrl: './share-list.component.html',
  styleUrls: ['./share-list.component.scss']
})
export class ShareListComponent implements OnInit {
  @Input() roomId: string = "0";
  constructor(public dialog: MatDialog) { }

  ngOnInit(): void {
  }
  openDialog() {
    const dialogRef = this.dialog.open(ShareListDialogComponent, {
      width: '20rem',
      data: this.roomId
    });
    dialogRef.afterClosed().subscribe();
  }

}

@Component({
  selector: 'share-list',
  templateUrl: './share-list-dialog.component.html',
  styleUrls: ['./share-list-dialog.component.scss'],
})
export class ShareListDialogComponent implements OnInit {
  email = new FormControl('');
  filteredOptions: Observable<Promise<string[]>> | undefined;
  progressBarShow: boolean = false;

  public roomId: string = "";
  public users: Array<User> = [];
  constructor(
    public dialogRef: MatDialogRef<ShareListDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: string, private _api: RoomsApiService
  ) {
    this.roomId = data;
  }

  public async ngOnInit() {
    await this.getSharedUsers();
    this.filteredOptions = this.email.valueChanges.pipe(
      startWith(''),
      map(async value => await this.filter(value || ''))
    );
  }

  public async getSharedUsers(){
    this.progressBarShow = true;
    this.users = await this._api.getRoomShares(this.roomId);
    this.progressBarShow = false;
  }

  public async filter(value: string):Promise<string[]> {
    let options = await this._api.getEmailsByFragment(value);

    const filterValue = value.toLowerCase();
    
    return options.filter(option => option.toLowerCase().includes(filterValue));
  }

  public async removeShare(uuid: string){
    await this._api.removeRoomShare(this.roomId, uuid);
    this.getSharedUsers();
  }

  public async shareRoom(){
    console.log(this.email.value);
    if(this.email.value != undefined){
      await this._api.shareRoom(this.roomId, this.email.value);
      this.getSharedUsers();
      this.email.setValue("");
    }
  }
};