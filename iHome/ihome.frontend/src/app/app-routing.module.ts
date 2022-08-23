import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '@auth0/auth0-angular';
import { ProfileComponent } from './pages/profile/profile.component';
import { RoomsComponent } from './pages/rooms/rooms.component';

const routes: Routes = [
  { path: 'rooms', component: RoomsComponent, canActivate: [AuthGuard],},
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard],}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
