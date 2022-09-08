import { Component, OnInit } from '@angular/core';
import { RoomsApiService } from 'src/app/services/rooms-api.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  constructor(private _api: RoomsApiService) { }

  public ngOnInit(): void {
    this.getBills();
  }

  public async getBills(){
    const res = await this._api.getBills();
  }

}
