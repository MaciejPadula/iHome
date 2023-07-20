import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IndexComponent } from './pages/index/index.component';
import { RoomsComponent } from './pages/rooms/rooms.component';
import { SchedulesComponent } from './pages/schedules/schedules.component';
import { AddScheduleComponent } from './pages/add-schedule/add-schedule.component';
import { RoomComponent } from './components/room/room.component';
import { Auth0AccessGuard } from './guards/auth0-access.guard';
import { AddRoomComponent } from './pages/add-room/add-room.component';

const routes: Routes = [
  {
    path: '',
    component: IndexComponent,
    data: { animation: 'HomePage' }
  },
  {
    path: 'rooms',
    component: RoomsComponent,
    canActivate: [Auth0AccessGuard],
    data: { animation: 'RoomsPage' },
  },
  {
    path: 'rooms/:id',
    component: RoomComponent,
    canActivate: [Auth0AccessGuard],
    data: { animation: 'RoomPage' }
  },
  {
    path: 'add-room',
    component: AddRoomComponent,
    canActivate: [Auth0AccessGuard],
    data: { animation: 'AddRoomPage' }
  },
  {
    path: 'schedules',
    component: SchedulesComponent,
    canActivate: [Auth0AccessGuard],
    data: { animation: 'SchedulesPage' }
  },
  {
    path: 'add-schedule',
    component: AddScheduleComponent,
    canActivate: [Auth0AccessGuard],
    data: { animation: 'AddSchedulePage' }
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
