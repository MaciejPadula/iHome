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
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSidenavModule } from '@angular/material/sidenav';

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
import { IndexComponent } from './pages/index/index.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { environment } from 'src/environments/environment';
import { AddDeviceComponent, AddDeviceDialogComponent } from './components/add-device/add-device.component';
import { AvailableDeviceComponent } from './components/add-device/available-device/available-device.component';
import { SidenavComponent } from './components/sidenav/sidenav.component';
import { IndexButtonComponent } from './components/sidenav/index-button/index-button.component';
import { RoomComponent } from './components/room/room.component';
import { DeviceComponent } from './components/device/device.component';
import { DeviceDialogComponent, RgbLampComponent } from './components/device/rgb-lamp/rgb-lamp.component';
import { ThermometerComponent } from './components/device/thermometer/thermometer.component';

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
    IndexComponent,
    NotFoundComponent,
    AddDeviceComponent,
    AddDeviceDialogComponent,
    AvailableDeviceComponent,
    SidenavComponent,
    IndexButtonComponent,
    RoomComponent,
    RgbLampComponent,
    DeviceDialogComponent,
    ThermometerComponent
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
    MatProgressSpinnerModule,
    MatProgressBarModule,
    MatSidenavModule,
    DragDropModule,
    HttpClientModule,
    AuthModule.forRoot({
      domain: environment.Auth0Domain,
      clientId: environment.ClientId,
      audience: environment.BackendUrl + environment.ApiSuffix,
      scope: environment.ApiScopes,
      httpInterceptor: {
        allowedList: [environment.BackendUrl + environment.ApiSuffix + '/*']
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
