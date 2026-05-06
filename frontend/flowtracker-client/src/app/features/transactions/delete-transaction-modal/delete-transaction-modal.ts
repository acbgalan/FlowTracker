import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnDestroy,
  Output,
  ViewChild,
  inject,
} from '@angular/core';
import { TransactionResponse } from '../../../core/models/transaction/transactionResponse.interface';
import { TransactionService } from '../../../core/services/transaction.service';

@Component({
  selector: 'app-delete-transaction-modal',
  imports: [],
  templateUrl: './delete-transaction-modal.html',
  styleUrl: './delete-transaction-modal.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DeleteTransactionModal implements OnDestroy {
  @ViewChild('dismissButton') dismissButton!: ElementRef<HTMLButtonElement>;
  private toastTimeoutId: number | null = null;

  private transactionService = inject(TransactionService);

  @Input() public transaction: TransactionResponse | null = null;
  @Output() public deleted = new EventEmitter<void>();
  public isDeleting = false;
  public errorMessage = '';
  public showSuccessToast = false;

  public closeSuccessToast(): void {
    this.showSuccessToast = false;
  }

  public onDelete(): void {
    const selectedTransaction = this.transaction;

    if (!selectedTransaction || this.isDeleting) {
      return;
    }

    this.isDeleting = true;
    this.errorMessage = '';

    this.transactionService.deleteTransaction(selectedTransaction.id).subscribe({
      next: () => {
        this.isDeleting = false;
        this.deleted.emit();
        this.hideModal();
        this.showSuccessToastMessage();
      },
      error: () => {
        this.isDeleting = false;
        this.errorMessage = 'No se pudo eliminar la transacción. Intenta nuevamente.';
      },
    });
  }

  private hideModal(): void {
    this.dismissButton.nativeElement.click();
  }

  private showSuccessToastMessage(): void {
    this.showSuccessToast = true;

    if (this.toastTimeoutId !== null) {
      window.clearTimeout(this.toastTimeoutId);
    }

    this.toastTimeoutId = window.setTimeout(() => {
      this.showSuccessToast = false;
      this.toastTimeoutId = null;
    }, 3000);
  }

  public ngOnDestroy(): void {
    if (this.toastTimeoutId !== null) {
      window.clearTimeout(this.toastTimeoutId);
      this.toastTimeoutId = null;
    }
  }

}
