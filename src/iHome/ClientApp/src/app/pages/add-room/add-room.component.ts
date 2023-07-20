import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RoomsService } from 'src/app/services/rooms.service';

@Component({
  selector: 'app-add-room',
  templateUrl: './add-room.component.html',
  styleUrls: ['./add-room.component.scss']
})
export class AddRoomComponent {
  addRoomStepperFirstFormGroup = this._formBuilder.group({
    roomName: ['', [Validators.required, Validators.minLength(3)]],
  });

  constructor(
    private _formBuilder: FormBuilder,
    private _roomsService: RoomsService,
    private _router: Router
  ) {}

  public addRoom() {
    const name = this.addRoomStepperFirstFormGroup.value.roomName;

    if(!name) return;

    this._roomsService.addRoom(name)
        .subscribe(() => this._router.navigate(['/rooms']));
  }
}
