import { TestBed } from '@angular/core/testing';

import { SchedulesBehaviourService } from './schedules-behaviour.service';

describe('SchedulesBehaviourService', () => {
  let service: SchedulesBehaviourService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SchedulesBehaviourService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
