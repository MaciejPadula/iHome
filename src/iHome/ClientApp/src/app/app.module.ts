import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AuthHttpInterceptor, AuthModule } from '@auth0/auth0-angular';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

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
    DevicesSidenavComponent
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
    MatRadioModule,
    MatCheckboxModule,
    MatSidenavModule,
    DragDropModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
