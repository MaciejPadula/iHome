import { ErrorHandler, NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';
import { AuthHttpInterceptor, AuthModule } from '@auth0/auth0-angular';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RoomsComponent } from './pages/rooms/rooms.component';
import { RoomComponent } from './pages/room/room.component';

import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from "@angular/material/button";
import { NavbarComponent } from './layout/navbar/navbar.component';
import { UserComponent } from './layout/navbar/user/user.component';
import { DeviceComponent } from './features/devices/device/device.component';
import { MatMenuModule } from '@angular/material/menu';
import { IndexComponent } from './pages/index/index.component';
import { WidgetComponent } from './features/widgets/widget/widget.component';
import { AddRoomDialogComponent } from './features/rooms/add-room-dialog/add-room-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule }from '@angular/material/input';
import { AddWidgetDialogComponent } from './features/widgets/add-widget-dialog/add-widget-dialog.component';
import { MatRadioModule } from '@angular/material/radio';
import { RenameRoomDialogComponent } from './features/rooms/rename-room-dialog/rename-room-dialog.component';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { environment } from 'src/environments/environment';
import { MatSidenavModule } from '@angular/material/sidenav';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { ShareRoomDialogComponent } from './features/rooms/share-room-dialog/share-room-dialog.component';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { ColorPickerModule } from '@iplab/ngx-color-picker';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { SchedulesComponent } from './pages/schedules/schedules.component';
import { ScheduleComponent } from './features/schedules/schedule/schedule.component';
import { InputDialogComponent } from './shared/components/input-dialog/input-dialog.component';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatDividerModule } from '@angular/material/divider';
import { NgxMatTimepickerModule } from 'ngx-mat-timepicker';
import { ConfirmDialogComponent } from './shared/components/confirm-dialog/confirm-dialog.component';
import { MatStepperModule } from '@angular/material/stepper';
import { AddScheduleComponent } from './pages/add-schedule/add-schedule.component';
import { DragDevicesListComponent } from './features/widgets/drag-devices-list/drag-devices-list.component';
import { NgxMatTimelineModule } from "ngx-mat-timeline";
import { ScheduleTitleComponent } from './shared/components/schedule-title/schedule-title.component';
import { RgbLampComponent } from './features/devices/device-dialog/rgb-lamp/rgb-lamp.component';
import { ScheduleDeviceComponent } from './features/schedules/schedule-device/schedule-device.component';
import { DevicePreviewComponent } from './features/devices/device-preview/device-preview.component';
import { DeviceDialogComponent } from './features/devices/device-dialog/device-dialog.component';
import { RgbLampPreviewComponent } from './features/devices/device-preview/rgb-lamp-preview/rgb-lamp-preview.component';
import { ThermometerPreviewComponent } from './features/devices/device-preview/thermometer-preview/thermometer-preview.component';
import { SchedulesPresentationComponent } from './features/devices/schedules-presentation/schedules-presentation.component';
import { ErrorDialogComponent } from './features/errors/error-dialog/error-dialog.component';
import { GlobalErrorHandler } from './features/errors/global-error-handler';
import { Auth0AccessGuard } from './features/guards/auth0-access.guard';
import { UserSearchComponent } from './shared/components/user-search/user-search.component';
import { MatChipsModule } from '@angular/material/chips';
import { SchedulesTimelineComponent } from './shared/components/schedules-timeline/schedules-timeline.component';
import { DeviceSchedulesTimelineComponent } from './features/devices/schedules-presentation/device-schedules-timeline/device-schedules-timeline.component';
import { MatSelectModule } from '@angular/material/select';
import { RoomRowComponent } from './features/rooms/room-row/room-row.component';


@NgModule({
  declarations: [
    AppComponent,
    RoomsComponent,
    RoomComponent,
    NavbarComponent,
    UserComponent,
    DeviceComponent,
    IndexComponent,
    WidgetComponent,
    AddRoomDialogComponent,
    AddWidgetDialogComponent,
    RenameRoomDialogComponent,
    ShareRoomDialogComponent,
    RgbLampComponent,
    ErrorDialogComponent,
    SchedulesComponent,
    ScheduleComponent,
    ScheduleDeviceComponent,
    InputDialogComponent,
    ConfirmDialogComponent,
    DevicePreviewComponent,
    AddScheduleComponent,
    DragDevicesListComponent,
    DeviceDialogComponent,
    RgbLampPreviewComponent,
    ThermometerPreviewComponent,
    ScheduleTitleComponent,
    SchedulesPresentationComponent,
    UserSearchComponent,
    SchedulesTimelineComponent,
    DeviceSchedulesTimelineComponent,
    RoomRowComponent
  ],
  imports: [
    BrowserModule,
    AuthModule.forRoot({
      domain: environment.authDomain,
      clientId: environment.authClientId,
      authorizationParams: {
        redirect_uri: window.location.origin,
        audience: environment.authAudience,
        scope: environment.authScope,
      },
      httpInterceptor: {
        allowedList: [
          {
            uri: `${environment.authAudience}/*`,
            tokenOptions: {
              authorizationParams: {
                audience: environment.authAudience,
                scope: environment.authScope
              }
            }
          }
        ]
      }
    }),
    BrowserAnimationsModule,
    HttpClientModule,
    AppRoutingModule,
    MatTabsModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatMenuModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule,
    MatRadioModule,
    MatCheckboxModule,
    MatSidenavModule,
    DragDropModule,
    MatProgressSpinnerModule,
    MatProgressBarModule,
    MatAutocompleteModule,
    ColorPickerModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatExpansionModule,
    MatDividerModule,
    NgxMatTimepickerModule,
    MatStepperModule,
    NgxMatTimelineModule,
    MatChipsModule,
    MatSelectModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true },
    { provide: ErrorHandler, useClass: GlobalErrorHandler },
    Auth0AccessGuard,
    provideClientHydration()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
