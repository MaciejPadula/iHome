import { TestBed } from '@angular/core/testing';

import { RoomsApiService } from './rooms-api.service';

describe('RoomsApiService', () => {
  let service: RoomsApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RoomsApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
