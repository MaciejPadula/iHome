import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { AddWidgetDialogComponent } from '../../features/widgets/add-widget-dialog/add-widget-dialog.component';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { WidgetType } from 'src/app/shared/models/widget-type';
import { RoomBehaviourService } from './service/room-behaviour.service';

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
  public roomId: string;
  public showDevicesList = false;

  constructor(
    private _roomBehaviourService: RoomBehaviourService,
    private _route: ActivatedRoute,
    private _dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this._route.params
      .pipe(untilDestroyed(this))
      .subscribe(params => {
        this.roomId = params['id'];
        this._roomBehaviourService.getWidgets(this.roomId);
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

  private addWidget(widgetType: WidgetType, showBorder: boolean) {
    this._roomBehaviourService.addWidget(this.roomId, widgetType, showBorder);
  }

  public get widgets() {
    return this._roomBehaviourService.widgets;
  }

  public get isLoading() {
    return this._roomBehaviourService.isLoading;
  }
}