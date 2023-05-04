import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IndexComponent } from './pages/index/index.component';
import { RoomsComponent } from './pages/rooms/rooms.component';
import { SchedulesComponent } from './pages/schedules/schedules.component';
import { AddScheduleComponent } from './pages/add-schedule/add-schedule.component';
import { RoomComponent } from './components/room/room.component';

const routes: Routes = [
  {
    path: '',
    component: IndexComponent,
    data: { animation: 'HomePage' }
  },
  {
    path: 'rooms',
    component: RoomsComponent,
    data: { animation: 'RoomsPage' }
  },
  {
    path: 'rooms/:id',
    component: RoomComponent,
    data: { animation: 'RoomPage' }
  },
  {
    path: 'schedules',
    component: SchedulesComponent,
    data: { animation: 'SchedulesPage' }
  },
  {
    path: 'add-schedule',
    component: AddScheduleComponent,
    data: { animation: 'AddSchedulePage' }
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
