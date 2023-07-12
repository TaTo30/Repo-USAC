import { TestBed } from '@angular/core/testing';

import { ServerConnectService } from './server-connect.service';

describe('ServerConnectService', () => {
  let service: ServerConnectService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ServerConnectService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
