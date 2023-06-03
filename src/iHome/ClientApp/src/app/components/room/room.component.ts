import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Subject } from 'rxjs';
import { Widget } from 'src/app/models/widget';
import { WidgetType } from 'src/app/models/widget-type';
import { RefreshService } from 'src/app/services/refresh.service';
import { WidgetsService } from 'src/app/services/widgets.service';
import { AddWidgetDialogComponent } from '../add-widget-dialog/add-widget-dialog.component';
import { animate, state, style, transition, trigger } from '@angular/animations';

@UntilDestroy()
@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.scss'],
  animations: [
    trigger('openClose', [
      state('open', style({
        height: '*',
      })),
      state('closed', style({
        height: '0',
      })),
      transition('* => *', [
        animate('0.2s')
      ])
    ])
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RoomComponent implements OnInit {
  private widgetsSubject$ = new Subject<Widget[]>();
  public widgets$ = this.widgetsSubject$.asObservable();

  public id: string;
  public showDevicesList = false;

  constructor(
    private _refreshService: RefreshService,
    private _widgetsService: WidgetsService,
    private _router: Router,
    private _route: ActivatedRoute,
    private _dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this._refreshService.refresh$
      .pipe(untilDestroyed(this))
      .subscribe(() => this.getWidgets());

    this._route.params
      .pipe(untilDestroyed(this))
      .subscribe(params => {
        this.id = params['id'];
        this._refreshService.refresh();
      });
  }

  public getWidgets(){
    if(!this.id) return;
    this._widgetsService.getWidgets(this.id)
      .subscribe({
        next: widgets => this.widgetsSubject$.next(widgets),
        error: () => this._router.navigate(['/rooms'])
      });
  }

  public composeAddWidgetDialog(){
    this._dialog.open(AddWidgetDialogComponent, {
      width: '600px',
    }).afterClosed()
      .subscribe(data => {
        if(data == null) return;

        this.addWidget(data.widgetType, data.showBorder);
      });
  }

  public toggleDevicesList(){
    this.showDevicesList = !this.showDevicesList;
  }

  private addWidget(widgetType: WidgetType, showBorder: boolean){
    this._widgetsService.addWidget(widgetType, this.id, showBorder)
      .subscribe(() => this._refreshService.refresh());
  }
}