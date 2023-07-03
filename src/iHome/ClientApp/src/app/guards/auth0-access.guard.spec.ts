import { TestBed } from '@angular/core/testing';

import { Auth0AccessGuard } from './auth0-access.guard';

describe('Auth0AccessGuard', () => {
  let guard: Auth0AccessGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(Auth0AccessGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
