import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AuthHttpInterceptor, AuthModule } from '@auth0/auth0-angular';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

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

@NgModule({
  declarations: [
    AppComponent,
    RoomsComponent,
    RoomComponent,
    NavbarComponent,
    UserComponent,
    DeviceComponent,
    IndexComponent
  ],
  imports: [
    BrowserModule,
    AuthModule.forRoot({
      domain: 'dev-e7eyj4xg.eu.auth0.com',
      clientId: 'eFHpoMFFdC7GXIfi9xe6VrZ5Z07xKl11',
      authorizationParams: {
        redirect_uri: window.location.origin,
        audience: 'https://localhost:32678/api',
        scope: 'openid profile email read:rooms write:rooms',
      },
      httpInterceptor: {
        allowedList: [
          {
            uri: 'https://localhost:32678/api/*',
            tokenOptions: {
              authorizationParams: {
                audience: 'https://localhost:32678/api',
                scope: 'openid profile email read:rooms write:rooms'
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
    MatMenuModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
