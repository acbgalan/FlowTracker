import { TestBed } from '@angular/core/testing';

import { SavingLogService } from './savingLog.service';

describe('SavingLogService', () => {
  let service: SavingLogService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SavingLogService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
