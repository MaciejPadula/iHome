import { TestBed } from '@angular/core/testing';

import { RoomBehaviourService } from './room-behaviour.service';

describe('RoomBehaviourService', () => {
  let service: RoomBehaviourService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RoomBehaviourService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
