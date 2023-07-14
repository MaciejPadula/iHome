import { TestBed } from '@angular/core/testing';

import { RoomsBehaviourService } from './rooms-behaviour.service';

describe('RoomsBehaviourService', () => {
  let service: RoomsBehaviourService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RoomsBehaviourService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
