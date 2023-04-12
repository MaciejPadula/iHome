import { ErrorHandler, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AuthHttpInterceptor, AuthModule } from '@auth0/auth0-angular';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RoomsComponent } from './pages/rooms/rooms.component';
import { RoomComponent } from './components/room/room.component';

import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from "@angular/material/button";
import { NavbarComponent } from './components/navbar/navbar.component';
import { UserComponent } from './components/navbar/user/user.component';
import { DeviceComponent } from './components/device/device.component';
import { MatMenuModule } from '@angular/material/menu';
import { IndexComponent } from './pages/index/index.component';
import { WidgetComponent } from './components/widget/widget.component';
import { AddRoomDialogComponent } from './components/add-room-dialog/add-room-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule }from '@angular/material/input';
import { AddWidgetDialogComponent } from './components/add-widget-dialog/add-widget-dialog.component';
import { MatRadioModule } from '@angular/material/radio';
import { RenameRoomDialogComponent } from './components/rename-room-dialog/rename-room-dialog.component';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { environment } from 'src/environments/environment';
import { DevicesSidenavComponent } from './components/devices-sidenav/devices-sidenav.component';
import { MatSidenavModule } from '@angular/material/sidenav';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { ThermometerComponent } from './components/device/thermometer/thermometer.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ShareRoomDialogComponent } from './components/share-room-dialog/share-room-dialog.component';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { RgbLampComponent } from './components/device/rgb-lamp/rgb-lamp.component';
import { ColorPickerModule } from '@iplab/ngx-color-picker';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { RgbLampDialogComponent } from './components/device/rgb-lamp/rgb-lamp-dialog/rgb-lamp-dialog.component';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { GlobalErrorHandler } from './errors/global-error-handler';
import { ErrorDialogComponent } from './errors/error-dialog/error-dialog.component';

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
    DevicesSidenavComponent,
    ThermometerComponent,
    ShareRoomDialogComponent,
    RgbLampComponent,
    RgbLampDialogComponent,
    ErrorDialogComponent
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
    FontAwesomeModule,
    MatAutocompleteModule,
    ColorPickerModule,
    MatSlideToggleModule,
    MatSnackBarModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true },
    { provide: ErrorHandler, useClass: GlobalErrorHandler }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
