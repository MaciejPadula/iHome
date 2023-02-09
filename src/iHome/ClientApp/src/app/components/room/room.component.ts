import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Subject } from 'rxjs';
import { Device } from 'src/app/models/device';
import { DevicesService } from 'src/app/services/devices.service';
import { RefreshService } from 'src/app/services/refresh.service';

@UntilDestroy()
@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RoomComponent implements OnInit {
  private devicesSubject$ = new Subject<Device[]>();
  public devices$ = this.devicesSubject$.asObservable();

  private id: string;

  constructor(
    private _devicesService: DevicesService,
    private _refreshService: RefreshService,
    private _route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this._refreshService.refresh$
      .pipe(untilDestroyed(this))
      .subscribe(_ => this.getDevices());

    this._route.params
      .pipe(untilDestroyed(this))
      .subscribe(params => {
        this.id = params['id'];
        this._refreshService.refresh();
      });
  }

  public getDevices(){
    if(!this.id) return;
    this._devicesService.getRoomDevices(this.id)
      .subscribe(devices => this.devicesSubject$.next(devices));
  }
}