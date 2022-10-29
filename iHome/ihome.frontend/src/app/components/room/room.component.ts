import { Component, Input, OnInit } from '@angular/core';
import { defaultRoom, Room } from 'src/app/models/room';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.scss']
})
export class RoomComponent implements OnInit {
  @Input() room: Room = defaultRoom;

  constructor() { }

  ngOnInit(): void {
  }

}
