import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteTransactionModal } from './delete-transaction-modal';

describe('DeleteTransactionModal', () => {
  let component: DeleteTransactionModal;
  let fixture: ComponentFixture<DeleteTransactionModal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeleteTransactionModal]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeleteTransactionModal);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
