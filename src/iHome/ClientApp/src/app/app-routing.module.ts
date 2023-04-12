import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IndexComponent } from './pages/index/index.component';
import { RoomsComponent } from './pages/rooms/rooms.component';
import { SchedulesComponent } from './pages/schedules/schedules.component';

const routes: Routes = [
  { path: '', component: IndexComponent },
  { path: 'rooms', component: RoomsComponent },
  { path: 'rooms/:id', component: RoomsComponent },
  { path: 'schedules', component: SchedulesComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
