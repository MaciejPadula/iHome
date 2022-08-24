import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatToolbarModule } from '@angular/material/toolbar';
import { NavbarComponent } from './components/navbar/navbar.component';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule, MAT_FORM_FIELD_DEFAULT_OPTIONS } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatTabsModule } from '@angular/material/tabs';
import { DragDropModule } from '@angular/cdk/drag-drop';

// Import the module from the SDK
import { AuthModule } from '@auth0/auth0-angular';
import { AuthHttpInterceptor } from '@auth0/auth0-angular';
import { HttpClientModule, HTTP_INTERCEPTORS  } from '@angular/common/http';
import { UserButtonComponent } from './components/user-button/user-button.component';
import { RoomsComponent } from './pages/rooms/rooms.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { AddRoomComponent, AddRoomDialogComponent } from './components/add-room/add-room.component';
import { RemoveRoomComponent, RemoveRoomDialogComponent } from './components/remove-room/remove-room.component';
import { ShareListComponent, ShareListDialogComponent } from './components/share-list/share-list.component';
import { DeviceComponent } from './pages/rooms/device/device.component';
import { RGBControlComponent } from './pages/rooms/device/rgbcontrol/rgbcontrol.component';
import { TermometerComponent } from './pages/rooms/device/termometer/termometer.component';

export function tokenGetter() {
  return localStorage.getItem("access_token");
}

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    UserButtonComponent,
    RoomsComponent,
    ProfileComponent,
    AddRoomComponent,
    AddRoomDialogComponent,
    RemoveRoomComponent,
    RemoveRoomDialogComponent,
    ShareListComponent,
    ShareListDialogComponent,
    DeviceComponent,
    RGBControlComponent,
    TermometerComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatTooltipModule,
    MatCardModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    MatAutocompleteModule,
    MatTabsModule,
    ReactiveFormsModule,
    MatSlideToggleModule,
    DragDropModule,
    HttpClientModule,
    AuthModule.forRoot({
      domain: 'dev-e7eyj4xg.eu.auth0.com',
      clientId: 'eFHpoMFFdC7GXIfi9xe6VrZ5Z07xKl11',
      audience: 'http://192.168.8.4:5000/api/Rooms',
      scope: 'openid profile email read:rooms write:rooms',
      httpInterceptor: {
        allowedList: ['http://192.168.8.4:5000/api/Rooms/*']
      }
    }),
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true },
    { provide: MAT_FORM_FIELD_DEFAULT_OPTIONS, useValue: {appearance: 'fill'}}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
