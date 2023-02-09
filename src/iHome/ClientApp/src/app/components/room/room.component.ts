import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Subject } from 'rxjs';
import { Device } from 'src/app/models/device';
import { Widget } from 'src/app/models/widget';
import { WidgetType } from 'src/app/models/widget-type';
import { DevicesService } from 'src/app/services/devices.service';
import { RefreshService } from 'src/app/services/refresh.service';
import { WidgetsService } from 'src/app/services/widgets.service';
import { AddWidgetDialogComponent } from '../add-widget-dialog/add-widget-dialog.component';

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

  private widgetsSubject$ = new Subject<Widget[]>();
  public widgets$ = this.widgetsSubject$.asObservable();

  private id: string;

  constructor(
    private _devicesService: DevicesService,
    private _refreshService: RefreshService,
    private _widgetsService: WidgetsService,
    private _router: Router,
    private _route: ActivatedRoute,
    private _dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this._refreshService.refresh$
      .pipe(untilDestroyed(this))
      .subscribe(_ => this.getWidgets());

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

  public getWidgets(){
    if(!this.id) return;
    this._widgetsService.getWidgets(this.id)
      .subscribe({
        next: widgets => this.widgetsSubject$.next(widgets),
        error: _ => this._router.navigate(['/rooms'])
      });
  }

  public composeAddWidgetDialog(){
    this._dialog.open(AddWidgetDialogComponent, {
      width: '600px',
    }).afterClosed()
      .subscribe(data => {
        if(data == null) return;

        this.addWidget(data.widgetType);
      });
  }

  private addWidget(widgetType: WidgetType){
    this._widgetsService.addWidget(widgetType, this.id)
      .subscribe(_ => this._refreshService.refresh());
  }
}