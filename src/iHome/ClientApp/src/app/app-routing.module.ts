import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IndexComponent } from './pages/index/index.component';
import { RoomsComponent } from './pages/rooms/rooms.component';

const routes: Routes = [
  { path: '', component: IndexComponent },
  { path: 'rooms', component: RoomsComponent },
  { path: 'rooms/:id', component: RoomsComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
