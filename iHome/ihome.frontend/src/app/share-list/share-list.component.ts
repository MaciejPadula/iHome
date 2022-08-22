import { Component, Inject, Input, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { RoomsApiService } from '../rooms-api.service';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { User } from '../user';

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
    dialogRef.afterClosed().subscribe(result => {
    });
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

  public roomId: number = 0;
  public users: Array<User> = [];
  constructor(
    public dialogRef: MatDialogRef<ShareListDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: string, private api: RoomsApiService
  ) {
    this.roomId = parseInt(data);
    this.api.getEmailsTest("*").subscribe(res => {
      this.options = res;
    });

    this.getSharedUsers();
  }

  ngOnInit() {
    this.filteredOptions = this.email.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value || '')),
    );
  }

  public getSharedUsers(){
    this.api.getRoomShares(this.roomId).subscribe(res => this.users = res);
  }

  public _filter(value: string):string[] {
    if(value.length<3){
      return [];
    }
    const filterValue = value.toLowerCase();

    return this.options.filter(option => option.toLowerCase().includes(filterValue));
  }

  removeShare(uuid: string){
    this.api.removeRoomShare(this.roomId, uuid).subscribe(res => this.getSharedUsers());
  }
};