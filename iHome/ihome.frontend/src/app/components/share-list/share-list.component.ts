import { Component, Inject, Input, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { User } from 'src/app/models/user';
import { RoomsApiService } from 'src/app/services/rooms-api.service';
import { ThisReceiver } from '@angular/compiler';

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
  options: Array<string> = [];
  filteredOptions: Observable<string[]> | undefined;
  progressBarShow: boolean = false;

  public roomId: number = 0;
  public users: Array<User> = [];
  constructor(
    public dialogRef: MatDialogRef<ShareListDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: string, private _api: RoomsApiService
  ) {
    this.roomId = parseInt(data);
  }

  public async ngOnInit() {
    this.options = await this._api.getEmailsByFragment("*");
    await this.getSharedUsers();
    this.filteredOptions = this.email.valueChanges.pipe(
      startWith(''),
      map(value => this.filter(value || '')),
    );
  }

  public async getSharedUsers(){
    this.progressBarShow = true;
    this.users = await this._api.getRoomShares(this.roomId);
    this.progressBarShow = false;
  }

  public filter(value: string):string[] {
    if(value.length<3){
      return [];
    }
    const filterValue = value.toLowerCase();

    return this.options.filter(option => option.toLowerCase().includes(filterValue));
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